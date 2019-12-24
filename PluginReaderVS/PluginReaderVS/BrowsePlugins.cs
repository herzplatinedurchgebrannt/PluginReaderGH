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
using System.Xml;
using delegateCommand.Model;
using Ookii.Dialogs.Wpf;

namespace PluginReaderVS
{
    class BrowsePlugins : INotifyPropertyChanged
    {

        public BrowsePlugins()
        {
            this.pluginPositions = new ObservableCollection<VSTPlugin>();
            this.pathPositions = new ObservableCollection<PathPlugin>();
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


        private ObservableCollection<PathPlugin> _pathPositions;
        public ObservableCollection<PathPlugin> pathPositions
        {
            get { return _pathPositions; }
            set
            {
                if (value != _pathPositions)
                {
                    _pathPositions = value;
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
                        int x = 0;
                        string nameOnly = "";
                        string pathOnly = "";

                        foreach (string vst in pathRootVST)
                        {
                            nameOnly = Path.GetFileName(pathRootVST[x]);
                            pathOnly = pathRootVST[x].Replace(nameOnly, "");

                            pluginPositions.Add(new VSTPlugin()
                            {
                                name = nameOnly,
                                path = pathOnly,
                                type = "dll",
                            });
                            x = x + 1;
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Fehler catch");
                }
            }
        }

        private DelegateCommand _xmlWriteCommand;
        public DelegateCommand xmlWriteCommand
        {
            get
            {
                if (_xmlWriteCommand == null)
                {
                    _xmlWriteCommand = new DelegateCommand(xmlWrite,

                        () =>
                        {
                            return true;
                        }
                        );
                }
                return _xmlWriteCommand;
            }
        }

        void xmlWrite()
        {
            if (pluginPositions.Count > 0)
            {
                VistaSaveFileDialog vfb = new VistaSaveFileDialog();

                Nullable<bool> dialogResult = vfb.ShowDialog();

                if (dialogResult == true)
                {
                    try
                    {
                        //string filename = @"D:\99_test\VSTPlugins.xml";

                        XmlTextWriter xmlWriter = new XmlTextWriter(vfb.FileName, System.Text.Encoding.UTF8);

                        xmlWriter.Formatting = Formatting.Indented;

                        xmlWriter.WriteStartDocument();

                        xmlWriter.WriteComment("Founded VST-Plugins. File created: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));

                        xmlWriter.WriteStartElement("VstPlugins");

                        for (int i = 0; i <= pluginPositions.Count() - 1; i++)
                        {
                            xmlWriter.WriteStartElement("Plugin");

                            xmlWriter.WriteElementString("name", pluginPositions[i].name);

                            xmlWriter.WriteElementString("type", pluginPositions[i].type);

                            xmlWriter.WriteElementString("path", pluginPositions[i].path);

                            xmlWriter.WriteEndElement();

                        }
                        xmlWriter.WriteEndElement();
                        xmlWriter.WriteEndDocument();
                        xmlWriter.Flush();
                        xmlWriter.Close();
                    }
                    catch
                    {
                        MessageBox.Show("Fehler catch");
                    }
                }
            }
        }


        private DelegateCommand _clearPluginPositionsCommand;
        public DelegateCommand clearPluginPositionsCommand
        {
            get
            {
                if (_clearPluginPositionsCommand == null)
                {
                    _clearPluginPositionsCommand = new DelegateCommand(clearPluginPositions,

                        () =>
                        {
                            return true;
                        }
                        );
                }
                return _clearPluginPositionsCommand;
            }
        }

        void clearPluginPositions()
        {
            pluginPositions.Clear();
            pathPositions.Clear();
        }

        private DelegateCommand _xmlPathReadCommand;
        public DelegateCommand xmlPathReadCommand
        {
            get
            {
                if (_xmlPathReadCommand == null)
                {
                    _xmlPathReadCommand = new DelegateCommand(xmlRead,

                        () =>
                        {
                            return true;
                        }
                        );
                }
                return _xmlPathReadCommand;
            }
        }

        void xmlRead()
        {
            VistaOpenFileDialog ofd = new VistaOpenFileDialog();
            Nullable<bool> dialogResult = ofd.ShowDialog();

            if (dialogResult == true)
            {
                try
                { 
                    XmlTextReader xtr = new XmlTextReader(ofd.FileName);

                    while (xtr.Read())
                    {
                        if (xtr.NodeType == XmlNodeType.Element && xtr.Name == "path")
                        {
                            pathPositions.Add(new PathPlugin()
                            {
                                path = xtr.ReadElementContentAsString()
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

        private DelegateCommand _xmlPathSaveCommand;
        public DelegateCommand xmlPathSaveCommand
        {
            get
            {
                if (_xmlPathSaveCommand == null)
                {
                    _xmlPathSaveCommand = new DelegateCommand(xmlSave,

                        () =>
                        {
                            return true;
                        }
                        );
                }
                return _xmlPathSaveCommand;
            }
        }

        void xmlSave()
        {
            if (pathPositions.Count > 0)
            {
                VistaSaveFileDialog vfb2 = new VistaSaveFileDialog();
                Nullable<bool> dialogResult = vfb2.ShowDialog();

                if (dialogResult == true)
                {
                    try
                    {
                        XmlTextWriter xmlWriter = new XmlTextWriter(vfb2.FileName + ".xml", System.Text.Encoding.UTF8);

                        xmlWriter.Formatting = Formatting.Indented;

                        xmlWriter.WriteStartDocument();

                        xmlWriter.WriteComment("Paths for VST-Plugins. File created: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));

                        xmlWriter.WriteStartElement("PluginPath");

                        for (int i = 0; i <= pathPositions.Count() - 1; i++)
                        {
                            xmlWriter.WriteStartElement("SystemPath");

                            xmlWriter.WriteElementString("path", pathPositions[i].path);

                            xmlWriter.WriteEndElement();

                        }
                        xmlWriter.WriteEndElement();
                        xmlWriter.WriteEndDocument();
                        xmlWriter.Flush();
                        xmlWriter.Close();
                    }
                    catch
                    {
                        MessageBox.Show("Fehler catch");
                    }
                }
            }
        }





        private DelegateCommand _browsePathPluginsCommand;
        public DelegateCommand browsePathPluginsCommand
        {
            get
            {
                if (_browsePathPluginsCommand == null)
                {
                    _browsePathPluginsCommand = new DelegateCommand(browsePathPlugins,

                        () =>
                        {
                            return true;
                        }
                        );
                }
                return _browsePathPluginsCommand;
            }
        }

        void browsePathPlugins()
        {
            if (pathPositions.Count > 0)
            {
                try
                {
                    foreach (PathPlugin p in pathPositions)
                    {
                        string hallo = p.path;

                        string[] pathRootVST = Directory.GetFiles(p.path, "*.dll");

                        if (pathRootVST.Length > 0)
                        {
                            int x = 0;
                            string nameOnly = "";
                            string pathOnly = "";

                            foreach (string vst in pathRootVST)
                            {
                                nameOnly = Path.GetFileName(pathRootVST[x]);
                                pathOnly = pathRootVST[x].Replace(nameOnly, "");

                                pluginPositions.Add(new VSTPlugin()
                                {
                                    name = nameOnly,
                                    path = pathOnly,
                                    type = "dll",
                                });
                                x = x + 1;
                            }



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
