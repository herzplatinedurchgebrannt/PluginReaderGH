using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using delegateCommand.Model;
using Ookii.Dialogs.Wpf;

namespace PluginReaderVS
{
    class BrowsePlugins : INotifyPropertyChanged
    {

        public BrowsePlugins()
        {
            this.pluginPositions = new ObservableCollection<VSTPlugin>();
        }


        private ObservableCollection<VSTPlugin> _pluginPositions;
        public ObservableCollection<VSTPlugin> pluginPositions
        {
            get { return _pluginPositions; }
            set
            {
                if (value != _pluginPositions)
                {
                    _pluginPositions = value;
                    NotifyPropertyChanged();
                }
            }
        }






        private DelegateCommand _browseCommand;
        public DelegateCommand browseCommand
        {
            get
            {
                if (_browseCommand == null)
                {
                    _browseCommand = new DelegateCommand(browsePlugins,

                        () =>
                        {
                            return true;
                        }
                        );
                }
                return _browseCommand;
            }
        }


        void browsePlugins()
        {
            Ookii.Dialogs.Wpf.VistaFolderBrowserDialog fbd = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            Nullable<bool> dialogResult = fbd.ShowDialog();

            if (dialogResult == true)
            {
                try
                {
                    string[] pathRootVST = Directory.GetFiles(fbd.SelectedPath.ToString(), "*.dll");

                    if (pathRootVST.Length > 0)
                    {
                        foreach (string vst in pathRootVST)
                        {
                            pluginPositions.Add(new VSTPlugin()
                            {
                                name = "hall0",
                                type = "irgendwas",
                                path = "hei und lo",
                            });
                        }



                    }
                }
                catch
                {
                    MessageBox.Show("Fehler catch");
                }
            }



        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Private Helpers

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {

            if (PropertyChanged != null)
            {

                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

            }
        }
        #endregion

    }
}
