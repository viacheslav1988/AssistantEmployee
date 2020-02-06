using AssistantEmployee.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AssistantEmployee.ViewModels
{
    class MainAboutViewModel : ViewModelBase
    {

        #region Поля

        private Assembly assembly;

        private string title;

        private string version;

        private string copyright;

        private ICommand closeCurrentWindowCommand;

        #endregion

        #region Конструкторы

        public MainAboutViewModel() => assembly = Assembly.GetExecutingAssembly();

        #endregion

        #region Свойства

        public string Title
        {
            get
            {
                if (title == null) title = assembly.GetAssemblyAttributePropertyValue<AssemblyTitleAttribute, string>("Title");
                return title;
            }
        }

        public string Version
        {
            get
            {
                if (version == null)
                {
                    StringBuilder result = new StringBuilder();
                    result.Append(assembly.GetName().Version.Major).Append('.');
                    if (assembly.GetName().Version.Minor == 0) result.Append('0');
                    else if (assembly.GetName().Version.Minor % 10 == 0) result.Append(assembly.GetName().Version.Minor / 10);
                    else result.AppendFormat("{0:00}", assembly.GetName().Version.Minor);
                    version = result.ToString();
                }
                return version;
            }
        }

        public string Copyright
        {
            get
            {
                if (copyright == null) copyright = assembly.GetAssemblyAttributePropertyValue<AssemblyCopyrightAttribute, string>("Copyright");
                return copyright;
            }
        }

        #endregion

        #region Команды

        public ICommand CloseCurrentWindowCommand
        {
            get
            {
                if (closeCurrentWindowCommand == null)
                    closeCurrentWindowCommand = new RelayCommand((parameter) => (parameter as Window)?.Close());
                return closeCurrentWindowCommand;
            }
            set { closeCurrentWindowCommand = value; }
        }

        #endregion

    }
}