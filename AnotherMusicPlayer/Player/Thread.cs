using Microsoft.Win32;
using NAudio.Wave;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace AnotherMusicPlayer
{
    public partial class Player
    {
        private static PowerModes LatestPowerMode = PowerModes.StatusChange;
        private static bool DoResume = false;

        /// <summary> Playing thread </summary>
        private static async void PlaySoundAsync(object file)
        {
            try
            {
                string FilePath = (string)file;
                bool started = false;
                AudioFileReader audioFile = null;
                Equalizer equalizer = null;
                WaveOutEvent outputDevice = null;
                long msval = 0;
                try { 
                    audioFile = new AudioFileReader(FilePath);
                    msval = audioFile.WaveFormat.AverageBytesPerSecond / 1000;
                    if (PlayNewPositions.ContainsKey(FilePath) && PlayNewPositions[FilePath] > -1) { audioFile.Position = PlayNewPositions[FilePath] * msval; }
                }
                catch (InvalidOperationException err)
                {
                    Debug.WriteLine("ERROR ==> VBR File detected, try file convertion");
                    try
                    {
                        string path2 = Path.GetTempFileName();
                        System.IO.File.Delete(path2);
                        path2 += ".mp3";

                        bool retC = await ConvExe(FilePath, path2);
                        if (retC == false)
                        {
                            Debug.WriteLine("ERROR ==> VBR File convertion failure");
                            PlaylistNext(); return;
                        }
                        FilesTags.SaveMediaInfo(path2, FilesTags.MediaInfo(FilePath, false), FilePath);
                        if (Settings.ConversionMode == 2) // Mode remplacement 
                        {
                            System.IO.File.Move(FilePath, FilePath + ".old");
                            System.IO.File.Move(path2, FilePath);
                            audioFile = new AudioFileReader(FilePath);
                            System.IO.File.Delete(FilePath + ".old");
                        }
                        else { audioFile = new AudioFileReader(path2); }
                    }
                    catch (Exception err2)
                    {
                        Debug.WriteLine("ERROR ==> VBR File convertion failure");
                        PlaylistNext(); return;
                    }
                }
                catch (Exception err) { Debug.WriteLine(JsonConvert.SerializeObject(err)); PlaylistNext(); return; }
                equalizer = new Equalizer((ISampleProvider)audioFile, EqualizerBands);
                outputDevice = new WaveOutEvent();

                //outputDevice.Init((IWaveProvider)audioFile);
                outputDevice.Init(equalizer);
                if (!AudioList.ContainsKey(FilePath)) { AudioList.Add(FilePath, audioFile); }

                int ret = -1; long ret2 = -1;

                //while (outputDevice.PlaybackState == PlaybackState.Playing)
                while (true)
                {
                    if (IsSuspended == true) { Thread.Sleep(100); continue; }

                    ret2 = ret = -1;
                    PlayStatus.TryGetValue(FilePath, out ret);
                    PlayNewPositions.TryGetValue(FilePath, out ret2);

                    if (DoResume)
                    {
                        outputDevice.Stop();
                        outputDevice.Dispose();
                        outputDevice = null;
                        equalizer = null;
                        audioFile.Close();
                        audioFile.Dispose();
                        audioFile = null;
                        DoResume = false;
                    }
                    if (outputDevice == null)
                    {
                        Debug.WriteLine("REPLAY !!!!");
                        audioFile = new AudioFileReader(FilePath);
                        equalizer = new Equalizer((ISampleProvider)audioFile, EqualizerBands);
                        outputDevice = new WaveOutEvent();
                        outputDevice.Init(equalizer);
                        if (!AudioList.ContainsKey(FilePath)) { AudioList.Add(FilePath, audioFile); }
                        audioFile.Position = Settings.LastPlaylistDuration;
                        outputDevice.Play();
                    }
                    equalizer.Update();

                    if (ret == 0 && outputDevice.PlaybackState == PlaybackState.Playing) { outputDevice.Pause(); _LatestPlayerStatus = PlayerStatus.Pause; }
                    if (ret == 1 && outputDevice.PlaybackState != PlaybackState.Playing)
                    {
                        outputDevice.Play(); CurrentFile = FilePath;
                        PlayerLengthChangedEventParams evtp = new PlayerLengthChangedEventParams();
                        evtp.duration = (long)(((AudioFileReader)audioFile).TotalTime.TotalMilliseconds);
                        LengthChanged(evtp);
                        if (started == false) { App.bdd.playCountUpdate(FilePath); }
                        started = true; 
                        _LatestPlayerStatus = PlayerStatus.Play;
                    }
                    if (ret == 2)
                    {
                        outputDevice.Stop();
                        if (IsSuspended == true)
                        {
                            PlayNewPositions[FilePath] = audioFile.Position / msval;
                            try { outputDevice.Stop(); } catch { }
                            outputDevice.Dispose();
                            outputDevice = null;
                            equalizer = null;
                            audioFile.Close();
                            audioFile.Dispose();
                            audioFile = null;
                            AudioList.Remove(FilePath);
                        }
                        else { break; }
                        _LatestPlayerStatus = PlayerStatus.Stop;
                    }
                    if (ret2 != -1)
                    {
                        audioFile.Position = ret2 * msval;
                        PlayNewPositions[FilePath] = -1;
                    }
                    if (IsSuspended == false)
                    {
                        try
                        {
                            PlayerPositionChangedEventParams evt = new PlayerPositionChangedEventParams();
                            evt.Position = (long)(audioFile.CurrentTime.TotalMilliseconds);
                            evt.duration = (long)(audioFile.TotalTime.TotalMilliseconds);
                            Settings.LastPlaylistDuration = evt.Position;
                            if (FilePath == CurrentFile) PositionChanged(evt);
                            if (outputDevice.PlaybackState == PlaybackState.Stopped && started == true && evt.Position > 0)
                            {
                                if (PlayRepeat || (PlayListIndex + 1 == PlayList.Count && PlayList.Count == 1))
                                {
                                    audioFile.Position = 0;
                                    PlayStatus[FilePath] = 1;
                                    outputDevice.Play();
                                }
                                else { PlaylistNext(); break; }
                            }

                            if (evt.Position >= evt.duration - 5000) { PlaylistPreloadNext(); }
                        }
                        catch(Exception) { break; }
                    }

                    Thread.Sleep(100);
                }

                // EndOfStreamException thread cleanup process 
                if (outputDevice != null) { 
                    try { outputDevice.Stop(); } catch (Exception) { } 
                    outputDevice.Dispose(); outputDevice = null; 
                }
                if (equalizer != null) { equalizer = null; }
                if (audioFile != null) { 
                    try { audioFile.Close(); } catch (Exception) { } 
                    audioFile.Dispose(); audioFile = null; 
                }
                PlayStatus.Remove(FilePath);
                PlayNewPositions.Remove(FilePath);
                if (AudioList.ContainsKey(FilePath)) { AudioList.Remove(FilePath); }
                ThreadList.Remove(FilePath);
            }
            catch (Exception error) { Debug.WriteLine(JsonConvert.SerializeObject(error)); }
            return;
        }

    }
}
