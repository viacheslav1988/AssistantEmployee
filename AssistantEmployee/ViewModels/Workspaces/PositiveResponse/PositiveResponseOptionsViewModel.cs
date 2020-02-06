using AssistantEmployee.CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace AssistantEmployee.ViewModels.Workspaces.PositiveResponse
{
    class PositiveResponseOptionsViewModel : WorkspaceOptionsBase
    {

        #region Поля

        private Dictionary<TextBox, (DispatcherTimer timer, string text)> textBoxesTimers;

        private enum CorrectionType { Default, Timers }

        private ICommand clearTextCommand;

        private ICommand updateTextLostFocusCommand;

        private ICommand updateTextEnterButtonCommand;

        private ICommand updateTextTimersCommand;

        #endregion

        #region Конструкторы

        public PositiveResponseOptionsViewModel()
        {
            NameDocument = "Положительный ответ";
            Properties.PositiveResponseSettings.Default.PropertyChanged += (sender, args) => OnPropertyChanged(args.PropertyName);
        }

        #endregion

        #region Свойства

        //Полный почтовый адрес
        public bool FullPostalAddress
        {
            set
            {
                if (Properties.PositiveResponseSettings.Default.FullPostalAddress == value) return;
                Properties.PositiveResponseSettings.Default.FullPostalAddress = value;
            }
            get { return Properties.PositiveResponseSettings.Default.FullPostalAddress; }
        }

        //Склонение имени
        public bool DeclensionFullName
        {
            set
            {
                if (Properties.PositiveResponseSettings.Default.DeclensionFullName == value) return;
                Properties.PositiveResponseSettings.Default.DeclensionFullName = value;
            }
            get { return Properties.PositiveResponseSettings.Default.DeclensionFullName; }
        }

        //Должность подписывающего
        public string PositionSignDocuments
        {
            set
            {
                if (Properties.PositiveResponseSettings.Default.PositionSignDocuments == value) return;
                Properties.PositiveResponseSettings.Default.PositionSignDocuments = value;
            }
            get { return Properties.PositiveResponseSettings.Default.PositionSignDocuments; }
        }

        //Ф.И.О. подписывающего
        public string NameSignDocuments
        {
            set
            {
                if (Properties.PositiveResponseSettings.Default.NameSignDocuments == value) return;
                Properties.PositiveResponseSettings.Default.NameSignDocuments = value;
            }
            get { return Properties.PositiveResponseSettings.Default.NameSignDocuments; }
        }

        //Показывать имя специалиста
        public bool ShowSpecialist
        {
            set
            {
                if (Properties.PositiveResponseSettings.Default.ShowSpecialist == value) return;
                Properties.PositiveResponseSettings.Default.ShowSpecialist = value;
            }
            get { return Properties.PositiveResponseSettings.Default.ShowSpecialist; }
        }

        //Имя специалиста
        public string NameSpecialist
        {
            set
            {
                if (Properties.PositiveResponseSettings.Default.NameSpecialist == value) return;
                Properties.PositiveResponseSettings.Default.NameSpecialist = value;
            }
            get { return Properties.PositiveResponseSettings.Default.NameSpecialist; }
        }

        #endregion

        #region Команды

        //Команда: очистить текст
        public ICommand ClearTextCommand
        {
            get
            {
                if (clearTextCommand == null) clearTextCommand = new RelayCommand(
                    (parameter) => (parameter as WatermarkTextBox)?.Clear(),
                    (parameter) => !string.IsNullOrEmpty((parameter as WatermarkTextBox)?.Text));
                return clearTextCommand;
            }
            set { clearTextCommand = value; }
        }

        //Команда: коррекция и обновление текста при потере фокуса
        public ICommand UpdateTextLostFocusCommand
        {
            get
            {
                if (updateTextLostFocusCommand == null) updateTextLostFocusCommand = new RelayCommand((parameter)
                    => CorrectionUpdateTextBox(parameter as WatermarkTextBox, CorrectionType.Default));
                return updateTextLostFocusCommand;
            }
            set { updateTextLostFocusCommand = value; }
        }

        //Команда: коррекция и обновление текста при нажатии клавиши Enter
        public ICommand UpdateTextEnterButtonCommand
        {
            get
            {
                if (updateTextEnterButtonCommand == null) updateTextEnterButtonCommand = new RelayCommand((parameter) =>
                {
                    if (Keyboard.IsKeyDown(Key.Enter))
                        CorrectionUpdateTextBox(parameter as WatermarkTextBox, CorrectionType.Default);
                });
                return updateTextEnterButtonCommand;
            }
            set { updateTextEnterButtonCommand = value; }
        }

        //Команда: коррекция и обновление текста текста по таймеру
        public ICommand UpdateTextTimersCommand
        {
            get
            {
                if (updateTextTimersCommand == null) updateTextTimersCommand = new RelayCommand(
                    (parameter) => BindTimersToTextBox(parameter as WatermarkTextBox));
                return updateTextTimersCommand;
            }
            set { updateTextTimersCommand = value; }
        }

        #endregion

        #region Методы

        public override void Cancel() => Properties.PositiveResponseSettings.Default.Reload();

        public override void Reset() => Properties.PositiveResponseSettings.Default.Reset();

        public override void Save() => Properties.PositiveResponseSettings.Default.Save();

        //Привязка таймеров к TextBox
        private void BindTimersToTextBox<T>(T textBox) where T : TextBox
        {
            if (textBox == null) return;
            if (textBoxesTimers == null) textBoxesTimers = new Dictionary<TextBox, (DispatcherTimer timer, string text)>();
            if (!textBoxesTimers.ContainsKey(textBox))
            {
                DispatcherTimer timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(2.0), Tag = textBox };
                timer.Tick += (sender, args) =>
                {
                    (sender as DispatcherTimer)?.Stop();
                    CorrectionUpdateTextBox((sender as DispatcherTimer)?.Tag as WatermarkTextBox, CorrectionType.Timers);
                };
                textBoxesTimers.Add(textBox, (timer, string.Empty));
            }
            textBox.TextChanged += (sender, args) =>
            {
                if (BindingOperations.GetBindingExpression(textBox, TextBox.TextProperty).HasError)
                {
                    Validation.ClearInvalid(BindingOperations.GetBindingExpression(textBox, TextBox.TextProperty));
                }
                DispatcherTimer timer = textBoxesTimers[textBox].timer;
                timer.Stop();
                timer.Start();
            };
        }

        //Коррекция и обновление текста в TexBox
        private void CorrectionUpdateTextBox<T>(T textBox, CorrectionType type) where T : TextBox
        {
            void MoveCarretToTheEnd(T t) => t.CaretIndex = t.Text.Length;
            if (textBox == null) return;
            if (type == CorrectionType.Timers && textBox.Text.Equals(textBoxesTimers[textBox].text)) return;
            if (Regex.IsMatch(textBox.Text, @"\s{2,}"))
            {
                textBox.Text = Regex.Replace(textBox.Text, @"\s{2,}", " ").Trim();
                MoveCarretToTheEnd(textBox);
            }
            else if (!string.IsNullOrEmpty(textBox.Text) && (textBox.Text[0] == ' ' || textBox.Text[textBox.Text.Length - 1] == ' '))
            {
                textBox.Text = textBox.Text.Trim();
                MoveCarretToTheEnd(textBox);
            }
            if (type == CorrectionType.Timers) textBoxesTimers[textBox] = (textBoxesTimers[textBox].timer, textBox.Text);
            textBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }

        #endregion

    }
}