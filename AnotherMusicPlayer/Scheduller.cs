using App;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Interop;
using TagLib;
using Timer = System.Timers.Timer;

namespace AnotherMusicPlayer
{
    public partial class Scheduller : Form
    {
        private Timer _timer = new Timer(5000); // Check every 5s
        private Dictionary<string, Func<SchedullerTaskItem, (bool, string)>> _functionalities = new Dictionary<string, Func<SchedullerTaskItem, (bool, string)>>();

        public Scheduller()
        {
            InitializeComponent();
            AnotherMusicPlayer.MainWindow2Space.Common.SetGlobalColor(this);
            //schedullerTaskItemBindingSource.Add(new SchedullerTaskItem() // Add test data
            //{
            //    Action = "AAA",
            //    ActionResume = "Change File Ratting (<my file path>)",
            //    Time = DateTime.Now,
            //    Status = "Pending"
            //});

            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();

            this.FormClosing += Scheduller_FormClosing;
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void Scheduller_FormClosing(object? sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        public void Stop() 
        {             
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
                _timer = null;
            }
        }

        public bool AddTask(SchedullerTaskItem item)
        {
            if (dataGridView1.InvokeRequired) { dataGridView1.Invoke(new Action(() => AddTask(item))); }
            else
            {
                schedullerTaskItemBindingSource.Add(item);
                dataGridView1.Refresh();
            }
            return true;
        }

        public SchedullerTaskItem SearchTask(string action, string details = null, string file = null)
        {
            if (dataGridView1.InvokeRequired) { dataGridView1.Invoke(new Action(() => SearchTask(action, details, file))); }
            else
            {
                SchedullerTaskItem? ret = null;
                List<SchedullerTaskItem> rl = schedullerTaskItemBindingSource.List.OfType<SchedullerTaskItem>().ToList();
                foreach(var item in rl)
                {
                    if (item._Status == SchedullerTaskItemStatus.Pending)
                    {
                        if (item.Action == action) {
                            bool ok = false;
                            if(details != null && item.Details == details) { ok = true; }
                            if(file != null && item.File == file) { ok = true; }
                            if(ok) { ret = item; }
                        }
                    }
                    if (ret != null) { break; }
                }
                if (ret != null) { return ret; }
            }
            return null;
        }

        private static Regex doubleRegex = new Regex(@"^[0-9]*,?[0-9]+$");

        private void _timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            if (dataGridView1.InvokeRequired) { dataGridView1.Invoke(new Action(() => _timer_Elapsed(sender, e))); }
            else
            {
                schedullerTaskItemBindingSource.List.OfType<SchedullerTaskItem>().ToList().ForEach(item =>
                {
                    if (item._Status == SchedullerTaskItemStatus.Pending)
                    {
                        if(_functionalities.ContainsKey(item.Action))
                        {
                            (bool l, string g) = _functionalities[item.Action].Invoke(item);
                            if (l) { item._Status = SchedullerTaskItemStatus.Completed; }
                            else { Debug.WriteLine(g); }
                        }
                        else
                        {
                            item._Status = SchedullerTaskItemStatus.Failed;
                            item.ActionResume += " - Action not found";
                        }
                    }
                });

                dataGridView1.Refresh();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.InvokeRequired) { dataGridView1.Invoke(new Action(() => button1_Click(sender, e))); }
            else
            {
                List<SchedullerTaskItem> itemsToRemove = new List<SchedullerTaskItem>();
                schedullerTaskItemBindingSource.List.OfType<SchedullerTaskItem>().ToList().ForEach(item =>
                {
                    if (item._Status != SchedullerTaskItemStatus.Pending)
                    { itemsToRemove.Add(item); }
                });
                foreach (var item in itemsToRemove) schedullerTaskItemBindingSource.Remove(item);
                dataGridView1.Refresh();
            }
        }

        public bool AddFunctionality(string actionName, Func<SchedullerTaskItem, (bool, string)> actionFunction)
        {
            _functionalities.Add(actionName, actionFunction);
            return true;
        }

        public bool HasFunctionality(string actionName)
        {
            return _functionalities.ContainsKey(actionName);
        }
    }

    public class SchedullerTaskItem
    {
        public DateTime Time { get; set; }
        public string ActionResume { get; set; } = "";
        public string Action { get; set; } = "";

        [IgnoreDataMember]
        public SchedullerTaskItemStatus _Status { get; set; }

        public string Status { 
            get { return _Status.ToString(); }
            set { _Status = EnumHelper<SchedullerTaskItemStatus>.Parse2(value) ?? SchedullerTaskItemStatus.Pending; }
        }

        public string Details { get; set; } = "";
        public string File { get; set; } = "";

        [IgnoreDataMember]
        public MJObject ExtraData { get; set; } = new MJObject();

        [IgnoreDataMember]
        public List<string> ParsedFiles { get; set; } = new List<string>();

        [IgnoreDataMember]
        public object ExtraObject { get; set; } = null;

        [IgnoreDataMember]
        public int Errors { get; set; } = 0;
    }

    public enum SchedullerTaskItemStatus
    {
        Pending = 0,
        Completed = 1,
        Failed = 2
    }
}
