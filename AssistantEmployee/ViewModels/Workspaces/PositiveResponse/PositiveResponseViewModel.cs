using AssistantEmployee.CustomControls;
using AssistantEmployee.Models.PositiveResponse;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace AssistantEmployee.ViewModels.Workspaces.PositiveResponse
{
    class PositiveResponseViewModel : WorkspaceBase, IDataErrorInfo
    {

        #region Поля

        private Point? point;

        private Dictionary<TextBox, (DispatcherTimer timer, string text)> textBoxesTimers;

        private enum CorrectionType { Default, Timers }

        private readonly PositiveResponseModel model;

        private DateTime? contractDateUnchecked;

        private DateTime? outgoingDateUnchecked;

        private DateTime? contractTerminationDateUnchecked;

        private DateTime? statementRecalculationDateUnchecked;

        private DateTime? startDateDiscountUnchecked;

        private DateTime? endDateDiscountUnchecked;

        private bool expanderOneOpen;

        private bool expanderTwoOpen;

        private bool expanderThreeOpen;

        private ICommand clearTextCommand;

        private ICommand clearDateCommand;

        private ICommand validateTextLostFocusCommand;

        private ICommand validateTextEnterButtonCommand;

        private ICommand validateTextTimersCommand;

        private ICommand updateDateChangedCommand;

        private ICommand documentMoveCommand;

        private ICommand validateDateDiscountPanelCommand;

        private ICommand applyDataCommand;

        private ICommand printDocumentCommand;

        #endregion

        #region Конструкторы

        public PositiveResponseViewModel()
        {
            model = new PositiveResponseModel();
            expanderOneOpen = true;
        }

        #endregion

        #region Свойства модели

        public string ContractNumber
        {
            set
            {
                if (model.ContractNumber == value) return;
                model.ContractNumber = value;
                AutofillOnPropertyChanged();
            }
            get { return model.ContractNumber; }
        }

        public DateTime? ContractDate
        {
            set
            {
                if (model.ContractDate == value) return;
                model.ContractDate = value;
                AutofillOnPropertyChanged();
            }
            get { return model.ContractDate; }
        }

        public string FullName
        {
            set
            {
                if (model.FullName == value) return;
                model.FullName = value;
                AutofillOnPropertyChanged();
            }
            get { return model.FullName; }
        }

        public Gender Gender
        {
            set
            {
                if (model.Gender == value) return;
                model.Gender = value;
                AutofillOnPropertyChanged();
            }
            get { return model.Gender; }
        }

        public string Address
        {
            set
            {
                if (model.Address == value) return;
                model.Address = value;
                AutofillOnPropertyChanged();
            }
            get { return model.Address; }
        }

        public string OutgoingNumber
        {
            set
            {
                if (model.OutgoingNumber == value) return;
                model.OutgoingNumber = value;
                AutofillOnPropertyChanged();
            }
            get { return model.OutgoingNumber; }
        }

        public DateTime? OutgoingDate
        {
            set
            {
                if (model.OutgoingDate == value) return;
                model.OutgoingDate = value;
                AutofillOnPropertyChanged();
            }
            get { return model.OutgoingDate; }
        }

        public bool StatementTermination
        {
            set
            {
                if (model.StatementTermination == value) return;
                model.StatementTermination = value;
                AutofillOnPropertyChanged();
            }
            get { return model.StatementTermination; }
        }

        public DateTime? ContractTerminationDate
        {
            set
            {
                if (model.ContractTerminationDate == value) return;
                model.ContractTerminationDate = value;
                AutofillOnPropertyChanged();
            }
            get { return model.ContractTerminationDate; }
        }

        public decimal AmountDebt
        {
            set
            {
                if (model.AmountDebt == value) return;
                model.AmountDebt = value;
                AutofillOnPropertyChanged();
            }
            get { return model.AmountDebt; }
        }

        public bool StatementRecalculation
        {
            set
            {
                if (model.StatementRecalculation == value) return;
                model.StatementRecalculation = value;
                AutofillOnPropertyChanged();
            }
            get { return model.StatementRecalculation; }
        }

        public DateTime? StatementRecalculationDate
        {
            set
            {
                if (model.StatementRecalculationDate == value) return;
                model.StatementRecalculationDate = value;
                AutofillOnPropertyChanged();
            }
            get { return model.StatementRecalculationDate; }
        }

        public DiscountType DiscountType
        {
            set
            {
                if (model.DiscountType == value) return;
                model.DiscountType = value;
                AutofillOnPropertyChanged();
            }
            get { return model.DiscountType; }
        }

        public Discounts Discounts
        {
            set
            {
                if (model.Discounts == value) return;
                model.Discounts = value;
                AutofillOnPropertyChanged();
            }
            get { return model.Discounts; }
        }

        public DateTime? StartDateDiscount
        {
            set
            {
                if (model.StartDateDiscount == value) return;
                model.StartDateDiscount = value;
                AutofillOnPropertyChanged();
            }
            get { return model.StartDateDiscount; }
        }

        public DateTime? EndDateDiscount
        {
            set
            {
                if (model.EndDateDiscount == value) return;
                model.EndDateDiscount = value;
                AutofillOnPropertyChanged();
            }
            get { return model.EndDateDiscount; }
        }

        public decimal AmountRecalculation
        {
            set
            {
                if (model.AmountRecalculation == value) return;
                model.AmountRecalculation = value;
                AutofillOnPropertyChanged();
            }
            get { return model.AmountRecalculation; }
        }

        #endregion

        #region Свойства модели без проверки

        public DateTime? ContractDateUnchecked
        {
            set
            {
                if (contractDateUnchecked == value) return;
                contractDateUnchecked = value;
                AutofillOnPropertyChanged();
            }
            get { return contractDateUnchecked; }
        }

        public DateTime? OutgoingDateUnchecked
        {
            set
            {
                if (outgoingDateUnchecked == value) return;
                outgoingDateUnchecked = value;
                AutofillOnPropertyChanged();
            }
            get { return outgoingDateUnchecked; }
        }

        public DateTime? ContractTerminationDateUnchecked
        {
            set
            {
                if (contractTerminationDateUnchecked == value) return;
                contractTerminationDateUnchecked = value;
                AutofillOnPropertyChanged();
            }
            get { return contractTerminationDateUnchecked; }
        }

        public DateTime? StatementRecalculationDateUnchecked
        {
            set
            {
                if (statementRecalculationDateUnchecked == value) return;
                statementRecalculationDateUnchecked = value;
                AutofillOnPropertyChanged();
            }
            get { return statementRecalculationDateUnchecked; }
        }

        public DateTime? StartDateDiscountUnchecked
        {
            set
            {
                if (startDateDiscountUnchecked == value) return;
                startDateDiscountUnchecked = value;
                AutofillOnPropertyChanged();
            }
            get { return startDateDiscountUnchecked; }
        }

        public DateTime? EndDateDiscountUnchecked
        {
            set
            {
                if (endDateDiscountUnchecked == value) return;
                endDateDiscountUnchecked = value;
                AutofillOnPropertyChanged();
            }
            get { return endDateDiscountUnchecked; }
        }

        #endregion

        #region Свойства расширителей

        public bool ExpanderOneOpen
        {
            set
            {
                if (expanderOneOpen == value) return;
                expanderOneOpen = value;
                if (value) { ExpanderTwoOpen = false; ExpanderThreeOpen = false; }
                AutofillOnPropertyChanged();
            }
            get { return expanderOneOpen; }
        }

        public bool ExpanderTwoOpen
        {
            set
            {
                if (expanderTwoOpen == value) return;
                expanderTwoOpen = value;
                if (value) { ExpanderOneOpen = false; ExpanderThreeOpen = false; }
                AutofillOnPropertyChanged();
            }
            get { return expanderTwoOpen; }
        }

        public bool ExpanderThreeOpen
        {
            set
            {
                if (expanderThreeOpen == value) return;
                expanderThreeOpen = value;
                if (value) { ExpanderOneOpen = false; ExpanderTwoOpen = false; }
                AutofillOnPropertyChanged();
            }
            get { return expanderThreeOpen; }
        }

        #endregion

        #region Команды

        //Команда: очистить текст
        public ICommand ClearTextCommand
        {
            get
            {
                return clearTextCommand ?? (clearTextCommand = new RelayCommand(
                    (parameter) => (parameter as WatermarkTextBox)?.Clear(),
                    (parameter) => !string.IsNullOrEmpty((parameter as WatermarkTextBox)?.Text)));
            }
            set { clearTextCommand = value; }
        }

        //Команда: очистить дату
        public ICommand ClearDateCommand
        {
            get
            {
                return clearDateCommand ?? (clearDateCommand = new RelayCommand(
                    (parameter) => (parameter as WatermarkDatePicker)?.Clear(),
                    (parameter) => (parameter as WatermarkDatePicker)?.SelectedDate == null ? false : true));
            }
            set { clearDateCommand = value; }
        }

        //Команда: проверка и коррекция текста при потере фокуса
        public ICommand ValidateTextLostFocusCommand
        {
            get
            {
                return validateTextLostFocusCommand ?? (validateTextLostFocusCommand = new RelayCommand(
                    (parameter) => CorrectionValidateTextBox(parameter as WatermarkTextBox, CorrectionType.Default)));
            }
            set { validateTextLostFocusCommand = value; }
        }

        //Команда: проверка и коррекция текста при нажатии клавиши Enter
        public ICommand ValidateTextEnterButtonCommand
        {
            get
            {
                return validateTextEnterButtonCommand ?? (validateTextEnterButtonCommand = new RelayCommand((parameter) =>
                {
                    if (Keyboard.IsKeyDown(Key.Enter)) CorrectionValidateTextBox(parameter as WatermarkTextBox, CorrectionType.Default);
                }));
            }
            set { validateTextEnterButtonCommand = value; }
        }

        //Команда: проверка и коррекция текста по таймеру
        public ICommand ValidateTextTimersCommand
        {
            get
            {
                return validateTextTimersCommand ?? (validateTextTimersCommand = new RelayCommand(
                    (parameter) => BindTimersToTextBox(parameter as WatermarkTextBox)));
            }
            set { validateTextTimersCommand = value; }
        }

        //Команда: Обновление привязки при изменение свойства DatePicker
        public ICommand UpdateDateChangedCommand
        {
            get
            {
                return updateDateChangedCommand ?? (updateDateChangedCommand = new RelayCommand(
                    (parameter) => (parameter as WatermarkDatePicker)?.GetBindingExpression(WatermarkDatePicker.SelectedDateProperty).UpdateSource()));
            }
            set { updateDateChangedCommand = value; }
        }

        //Команда: перемещение документа
        public ICommand DocumentMoveCommand
        {
            get
            {
                return documentMoveCommand ?? (documentMoveCommand = new RelayCommand(
                    (parameter) => DocumentMove(parameter as ScrollViewer)));
            }
            set { documentMoveCommand = value; }
        }

        //Команда: проверка даты скидок
        public ICommand ValidateDateDiscountPanelCommand
        {
            get
            {
                return validateDateDiscountPanelCommand ?? (validateDateDiscountPanelCommand = new RelayCommand(
                    (parameter) => (parameter as StackPanel)?.BindingGroup.CommitEdit()));
            }
            set { validateDateDiscountPanelCommand = value; }
        }

        //Команда: применение введенных данных
        public ICommand ApplyDataCommand
        {
            get
            {
                return applyDataCommand ?? (applyDataCommand = new RelayCommand((parameter) =>
                {
                    (parameter as List<FrameworkElement>)?.ForEach(e =>
                    {
                        if (e is WatermarkTextBox) e.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                        else if (e is WatermarkDatePicker)
                        {
                            GetType().GetProperty(e.Name)?.SetValue(this, GetType().GetProperty(e.Name + "Unchecked")?.GetValue(this));
                        }
                    });
                }, (paramater) => FieldValidation(paramater as List<FrameworkElement>)));
            }
            set { applyDataCommand = value; }
        }

        //Команда: печать документа
        public ICommand PrintDocumentCommand
        {
            get
            {
                return printDocumentCommand ?? (printDocumentCommand = new RelayCommand((parameter) =>
                {
                    //В РАЗРАБОТКЕ!!!
                }));
            }
            set { printDocumentCommand = value; }
        }

        #endregion

        #region Реализация интерфейса IDataErrorInfo

        public string this[string columnName] => ValidatingDates(columnName);

        public string Error => null;

        #endregion

        #region Методы

        //Перемещение документа
        private void DocumentMove<T>(T scrollViewer) where T : ScrollViewer
        {
            if (scrollViewer == null) return;
            switch(Mouse.LeftButton)
            {
                case MouseButtonState.Pressed:
                    {
                        if (Mouse.OverrideCursor != Cursors.Hand) Mouse.OverrideCursor = Cursors.Hand;
                        if (point == null) point = Mouse.GetPosition(scrollViewer);
                        scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - (Mouse.GetPosition(scrollViewer).X - ((Point)point).X));
                        scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - (Mouse.GetPosition(scrollViewer).Y - ((Point)point).Y));
                        point = Mouse.GetPosition(scrollViewer);
                        break;
                    }
                case MouseButtonState.Released:
                    {
                        if (Mouse.OverrideCursor != null) Mouse.OverrideCursor = null;
                        if (point != null) point = null;
                        break;
                    }
            }
        }

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
                    CorrectionValidateTextBox((sender as DispatcherTimer)?.Tag as WatermarkTextBox, CorrectionType.Timers);
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

        //Коррекция и проверка текста в TexBox
        private void CorrectionValidateTextBox<T>(T textBox, CorrectionType type) where T : TextBox
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
            textBox.GetBindingExpression(TextBox.TextProperty).ValidateWithoutUpdate();
        }

        //Проверка дат
        private string ValidatingDates(string columnName)
        {
            const string OUTGOING_DATE_NOT_VALID = "Дата ответа меньше даты договора";
            const string CONTRACT_TERMINATION_DATE_NOT_VALID = "Дата расторжения меньше даты договора";
            const string STATEMENT_RECALCULATION_DATE_NOT_VALID = "Дата заявл. на перер. меньше даты дог.";
            const string CONTRACT_DATE_NOT_VALID = "Дата договора не определена";
            switch (columnName)
            {
                case nameof(OutgoingDateUnchecked):
                    {
                        if (ContractDateUnchecked != null &&
                            OutgoingDateUnchecked != null &&
                            ((TimeSpan)(OutgoingDateUnchecked - ContractDateUnchecked)).Days < 0) return OUTGOING_DATE_NOT_VALID;
                        else if (ContractDateUnchecked == null && OutgoingDateUnchecked != null) return CONTRACT_DATE_NOT_VALID;
                        break;
                    }
                case nameof(ContractTerminationDateUnchecked):
                    {
                        if (ContractDateUnchecked != null &&
                            ContractTerminationDateUnchecked != null &&
                            ((TimeSpan)(ContractTerminationDateUnchecked - ContractDateUnchecked)).Days < 0) return CONTRACT_TERMINATION_DATE_NOT_VALID;
                        else if (ContractDateUnchecked == null && ContractTerminationDateUnchecked != null) return CONTRACT_DATE_NOT_VALID;
                        break;
                    }
                case nameof(StatementRecalculationDateUnchecked):
                    {
                        if (ContractDateUnchecked != null &&
                            StatementRecalculationDateUnchecked != null &&
                            ((TimeSpan)(StatementRecalculationDateUnchecked - ContractDateUnchecked)).Days < 0) return STATEMENT_RECALCULATION_DATE_NOT_VALID;
                        else if (ContractDateUnchecked == null && StatementRecalculationDateUnchecked != null) return CONTRACT_DATE_NOT_VALID;
                        break;
                    }
            }
            return string.Empty;
        }

        //Проверка полей на корректность
        private bool FieldValidation<T>(List<T> list) where T : FrameworkElement
        {
            if (list == null) return false;
            string[][] elementNames =
            {
                new string[] { "DateDiscountPanel", "StartDateDiscount", "EndDateDiscount" },
                new string[] { "ContractTerminationDate", "AmountDebt", "AmountRecalculation" },
                new string[] { "ContractTerminationDate", "AmountDebt", "DateDiscountPanel", "StartDateDiscount", "EndDateDiscount" },
                new string[] { "ContractTerminationDate", "AmountDebt" }
            };
            foreach (FrameworkElement element in list)
            {
                if (StatementTermination)
                {
                    if (elementNames[0].Contains(element.Name)) continue;
                }
                else
                {
                    switch (DiscountType)
                    {
                        case DiscountType.Discount when elementNames[1].Contains(element.Name): continue;
                        case DiscountType.Recalculation when elementNames[2].Contains(element.Name): continue;
                        case DiscountType.DiscountAndRecalculation when elementNames[3].Contains(element.Name): continue;
                    }
                }
                switch (element)
                {
                    case WatermarkTextBox textBox:
                        if (textBox.GetBindingExpression(TextBox.TextProperty).HasError || (textBox.Name != "AmountDebt" && string.IsNullOrEmpty(textBox.Text))) return false;
                        break;
                    case WatermarkDatePicker datePicker:
                        if (datePicker.GetBindingExpression(WatermarkDatePicker.SelectedDateProperty).HasError || datePicker.SelectedDate == null) return false;
                        break;
                    case StackPanel stackPanel:
                        if (stackPanel.BindingGroup.HasValidationError) return false;
                        break;
                }
            }
            return true;
        }

        #endregion

    }
}