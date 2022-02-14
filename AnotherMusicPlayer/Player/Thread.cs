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
        /// <summary> Playing thread </summary>
        private async void PlaySoundAsync(object file)
        {
            try
            {
                string FilePath = (string)file;
                bool started = false;
                AudioFileReader audioFile = null;
                Equalizer equalizer = null;
                WaveOutEvent outputDevice = null;

                try { audioFile = new AudioFileReader(FilePath); }
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
                        System.IO.File.Move(FilePath, FilePath + ".old");
                        System.IO.File.Move(path2, FilePath);
                        audioFile = new AudioFileReader(FilePath);
                        System.IO.File.Delete(FilePath + ".old");
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
                    ret2 = ret = -1;
                    PlayStatus.TryGetValue(FilePath, out ret);
                    PlayNewPositions.TryGetValue(FilePath, out ret2);

                    if (outputDevice == null)
                    {
                        if (IsSuspended == true) { Thread.Sleep(100); continue; }
                        else
                        {
                            Debug.WriteLine("REPLAY !!!!");
                            audioFile = new AudioFileReader(FilePath);
                            equalizer = new Equalizer((ISampleProvider)audioFile, EqualizerBands);
                            outputDevice = new WaveOutEvent();
                            outputDevice.Init(equalizer);
                            if (!AudioList.ContainsKey(FilePath)) { AudioList.Add(FilePath, audioFile); }
                            outputDevice.Play();
                        }
                    }
                    equalizer.Update();

                    if (ret == 0 && outputDevice.PlaybackState == PlaybackState.Playing) { outputDevice.Pause(); }
                    if (ret == 1 && outputDevice.PlaybackState != PlaybackState.Playing)
                    {
                        outputDevice.Play(); CurrentFile = FilePath;
                        PlayerLengthChangedEventParams evtp = new PlayerLengthChangedEventParams();
                        evtp.duration = (long)(((AudioFileReader)audioFile).TotalTime.TotalMilliseconds);
                        LengthChanged(this, evtp);
                        if (started == false) { parent.bdd.playCountUpdate(FilePath); }
                        started = true;
                    }
                    if (ret == 2)
                    {
                        outputDevice.Stop();
                        if (IsSuspended == true)
                        {
                            long msval = audioFile.WaveFormat.AverageBytesPerSecond / 1000;
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
                    }
                    if (ret2 != -1)
                    {
                        long msval = audioFile.WaveFormat.AverageBytesPerSecond / 1000;
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
                            if (FilePath == CurrentFile) PositionChanged(this, evt);
                            if (outputDevice.PlaybackState == PlaybackState.Stopped && started == true && evt.Position > 0)
                            {
                                if (PlayRepeat || (PlayListIndex + 1 == PlayList.Count && PlayList.Count == 1))
                                {
                                    audioFile.Position = 0;
                                    PlayStatus[FilePath] = 1;
                                    outputDevice.Play();
                                }
                                else
                                {
                                    PlaylistNext();
                                    break;
                                }
                            }

                            if (evt.Position >= evt.duration - 5000) { PlaylistPreloadNext(); }
                        }
                        catch { break; }
                    }

                    Thread.Sleep(100);
                }
                try { outputDevice.Stop(); } catch { }

                outputDevice.Dispose();
                ((AudioFileReader)audioFile).Close(); ((AudioFileReader)audioFile).Dispose();
                PlayStatus.Remove(FilePath);
                PlayNewPositions.Remove(FilePath);
                if (AudioList.ContainsKey(FilePath))
                {
                    ((AudioFileReader)AudioList[FilePath]).Dispose();
                    AudioList.Remove(FilePath);
                }
                ThreadList.Remove(FilePath);
            }
            catch (Exception error) { Debug.WriteLine(JsonConvert.SerializeObject(error)); }
            return;
        }

    }
}
