using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PluginInstaller
{
    public partial class Form1 : Form
    {
        private const string TargetBasePath = @"C:\Program Files\CSoft\Model Studio CS\3.1\MIA\bin";
        private const string TargetPluginsPath = TargetBasePath + @"\Plugins";
        private const string PluginsXmlPath = TargetBasePath + @"\plugins.xml";

        private string _localPluginsDir;
        private List<PluginControl> _pluginControls = new List<PluginControl>();

        public Form1()
        {
            InitializeComponent();
            _localPluginsDir = Path.Combine(Application.StartupPath, "Plugins");
            LoadPlugins();
            SetWindowIcon();
        }

        private void SetWindowIcon()
        {
            string logoPath = Path.Combine(Application.StartupPath, "logo.png");
            if (!File.Exists(logoPath))
                return;

            try
            {
                using (var bitmap = new System.Drawing.Bitmap(logoPath))
                {

                    var resized = new System.Drawing.Bitmap(bitmap, new System.Drawing.Size(32, 32));
                    var icon = System.Drawing.Icon.FromHandle(resized.GetHicon());
                    this.Icon = icon;


                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка загрузки иконки: {ex.Message}");
            }
        }

        private void LoadPlugins()
        {
            flowLayoutPanelPlugins.Controls.Clear();
            _pluginControls.Clear();

            if (!Directory.Exists(_localPluginsDir))
            {
                MessageBox.Show($"Папка Plugins не найдена:\n{_localPluginsDir}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var pluginFolders = Directory.GetDirectories(_localPluginsDir).Select(Path.GetFileName).ToArray();

            foreach (string folder in pluginFolders)
            {
                var pluginCtrl = new PluginControl(folder, _localPluginsDir);
                string destPath = Path.Combine(TargetPluginsPath, folder);
                if (Directory.Exists(destPath))
                {
                    pluginCtrl.MarkAsInstalled();
                }
                flowLayoutPanelPlugins.Controls.Add(pluginCtrl);
                _pluginControls.Add(pluginCtrl);
            }
        }

        private void buttonInstall_Click(object sender, EventArgs e)
        {
            
            var selectedPlugins = _pluginControls.Where(pc => pc.IsSelected).ToList();

            if (!selectedPlugins.Any())
            {
                MessageBox.Show("Выберите хотя бы один плагин для установки.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            
            foreach (var pc in selectedPlugins)
            {
                if (!pc.IsValidForInstall())
                {
                    MessageBox.Show(pc.GetValidationMessage(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            
            try
            {
                Directory.CreateDirectory(TargetPluginsPath);

                XDocument doc;
                if (File.Exists(PluginsXmlPath))
                {
                    doc = XDocument.Load(PluginsXmlPath);
                }
                else
                {
                    doc = new XDocument(
                        new XDeclaration("1.0", "utf-8", null),
                        new XElement("Plugins", new XAttribute("logFolder", "Library3D"))
                    );
                }

                var pluginsElement = doc.Root;

                foreach (var pc in selectedPlugins)
                {
                    string sourceDir = Path.Combine(_localPluginsDir, pc.PluginName);
                    string destDir = Path.Combine(TargetPluginsPath, pc.PluginName);

                    if (Directory.Exists(destDir))
                        Directory.Delete(destDir, true);

                    CopyDirectory(sourceDir, destDir);

                    string fullDllPath = Path.Combine(destDir, pc.SelectedDllRelativePath.Replace('/', '\\'));

                    string dllFileName = Path.GetFileName(fullDllPath);
                    bool alreadyExists = pluginsElement.Elements("Plugin")
                        .Any(el =>
                        {
                            var nameAttr = el.Attribute("name")?.Value;
                            return nameAttr != null && Path.GetFileName(nameAttr) == dllFileName;
                        });

                    if (!alreadyExists)
                    {
                        pluginsElement.Add(new XElement("Plugin", new XAttribute("name", fullDllPath)));
                    }
                }

                doc.Save(PluginsXmlPath);
                MessageBox.Show("Плагины успешно установлены!", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadPlugins(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при установке:\n{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void CopyDirectory(string sourceDir, string targetDir)
        {
            Directory.CreateDirectory(targetDir);
            foreach (string file in Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories))
            {
                string relativePath = file.Substring(sourceDir.Length + 1);
                string destFile = Path.Combine(targetDir, relativePath);
                Directory.CreateDirectory(Path.GetDirectoryName(destFile)!);
                File.Copy(file, destFile, true);
            }
        }
    }

    
    public class PluginControl : UserControl
    {
        private CheckBox checkBox;
        private Label labelName;
        private Button buttonSelectDll;
        private string _pluginName;
        private string _localPluginsDir;
        private bool _isInstalled = false;

        public string PluginName => _pluginName;
        public bool IsSelected => checkBox.Checked;
        public string SelectedDllRelativePath { get; private set; }

        public PluginControl(string pluginName, string localPluginsDir)
        {
            _pluginName = pluginName;
            _localPluginsDir = localPluginsDir;
            InitializeComponent();
            UpdateDisplay();
            checkBox.CheckedChanged += (s, e) => OnSelectionChanged();
        }


        private void InitializeComponent()
        {
            this.checkBox = new CheckBox();
            this.labelName = new Label();
            this.buttonSelectDll = new Button();

            this.SuspendLayout();

            this.checkBox.Location = new System.Drawing.Point(3, 3);
            this.checkBox.Size = new System.Drawing.Size(18, 18);

            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(25, 5);
            this.labelName.MaximumSize = new System.Drawing.Size(250, 0);
            this.labelName.AutoEllipsis = true;

            this.buttonSelectDll.Text = "Выбрать DLL";
            this.buttonSelectDll.Size = new System.Drawing.Size(90, 23);
            this.buttonSelectDll.Location = new System.Drawing.Point(280, 2);
            this.buttonSelectDll.Visible = false; // Скрыта по умолчанию
            this.buttonSelectDll.Click += ButtonSelectDll_Click;

            this.Controls.Add(this.checkBox);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.buttonSelectDll);

            this.Size = new System.Drawing.Size(375, 30);
            this.ResumeLayout(false);
            this.PerformLayout();

        }




        private void OnSelectionChanged()
        {
            
            buttonSelectDll.Visible = checkBox.Checked;

            
        }

        private void ButtonSelectDll_Click(object sender, EventArgs e)
        {
            string pluginFullPath = Path.Combine(_localPluginsDir, _pluginName);
            if (!Directory.Exists(pluginFullPath))
            {
                MessageBox.Show("Папка плагина недоступна.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var dialog = new OpenFileDialog())
            {
                dialog.InitialDirectory = pluginFullPath;
                dialog.Filter = "DLL файлы (*.dll)|*.dll|Все файлы (*.*)|*.*";
                dialog.Title = $"Выберите основной DLL для '{_pluginName}'";
                dialog.RestoreDirectory = false;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (!dialog.FileName.StartsWith(pluginFullPath, StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("Можно выбирать только файлы внутри папки плагина.", "Недопустимый файл", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    SelectedDllRelativePath = dialog.FileName.Substring(pluginFullPath.Length + 1).Replace('\\', '/');
                    MessageBox.Show($"Выбран DLL:\n{SelectedDllRelativePath}", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        public void MarkAsInstalled()
        {
            _isInstalled = true;
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            labelName.Text = _isInstalled ? $"{_pluginName} (установлено)" : _pluginName;
        }

        public bool IsValidForInstall()
        {
            if (!IsSelected) return true; 
            return !string.IsNullOrEmpty(SelectedDllRelativePath);
        }

        public string GetValidationMessage()
        {
            if (IsSelected && string.IsNullOrEmpty(SelectedDllRelativePath))
                return $"Не выбран DLL для плагина '{_pluginName}'. Нажмите «Выбрать DLL».";
            return null;
        }
    }
}