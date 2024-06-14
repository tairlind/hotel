using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnSelectNumber_Click(object sender, RoutedEventArgs e)
        {
            // Открываем диалоговое окно для выбора числа
            InputDialog inputDialog = new InputDialog("Введите число от 1 до 100", "Выбор числа");
            if (inputDialog.ShowDialog() == true)
            {
                // Получаем выбранное число
                txtNumber.Text = inputDialog.InputText;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получаем выбранную дату
                DateTime selectedDate = calendar.SelectedDate.Value;

                // Получаем выбранное число
                int selectedNumber = int.Parse(txtNumber.Text);

                // Открываем диалоговое окно выбора папки
                using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
                {
                    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        // Получаем выбранный путь к папке
                        string selectedFolderPath = dialog.SelectedPath;

                        // Проверяем, находится ли число в диапазоне от 1 до 100
                        if (selectedNumber >= 1 && selectedNumber <= 100)
                        {
                            // Сохраняем данные в текстовый файл
                            string fileName = $"data_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                            string filePath = Path.Combine(selectedFolderPath, fileName);

                            using (StreamWriter writer = new StreamWriter(filePath))
                            {
                                writer.WriteLine($"Дата: {selectedDate:dd.MM.yyyy}");
                                writer.WriteLine($"Число: {selectedNumber}");
                            }

                            // Выводим сообщение об успешном сохранении
                            MessageBox.Show("Информация сохранена в файл!");
                        }
                        else
                        {
                            MessageBox.Show("Число должно быть от 1 до 100.");
                        }
                    }
                    else
                    {
                        // Пользователь отменил выбор папки
                        MessageBox.Show("Сохранение отменено.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Обрабатываем ошибки
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        // Внутренний класс для создания диалогового окна
        private class InputDialog : Window
        {
            public string InputText { get; private set; }

            public InputDialog(string message, string title)
            {
                // Создаем элементы диалогового окна
                TextBlock messageLabel = new TextBlock { Text = message, Margin = new Thickness(10) };
                TextBox inputTextBox = new TextBox { Margin = new Thickness(10) };
                Button okButton = new Button { Content = "OK", Margin = new Thickness(10), Width = 75 };
                Button cancelButton = new Button { Content = "Отмена", Margin = new Thickness(10), Width = 75 };

                // Настраиваем поведение кнопок
                okButton.Click += (s, e) =>
                {
                    InputText = inputTextBox.Text;
                    DialogResult = true;
                    Close();
                };

                cancelButton.Click += (s, e) =>
                {
                    DialogResult = false;
                    Close();
                };

                // Добавляем элементы в диалоговое окно
                Grid grid = new Grid { Margin = new Thickness(10) };
                grid.RowDefinitions.Add(new RowDefinition());
                grid.RowDefinitions.Add(new RowDefinition());
                grid.RowDefinitions.Add(new RowDefinition());

                grid.Children.Add(messageLabel);
                Grid.SetRow(messageLabel, 0);
                grid.Children.Add(inputTextBox);
                Grid.SetRow(inputTextBox, 1);

                // Добавляем кнопки в отдельный StackPanel
                StackPanel buttonPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(10) };
                buttonPanel.Children.Add(okButton);
                buttonPanel.Children.Add(cancelButton);
                grid.Children.Add(buttonPanel);
                Grid.SetRow(buttonPanel, 2);

                // Настраиваем диалоговое окно
                Content = grid;
                Title = title;
                SizeToContent = SizeToContent.WidthAndHeight;
            }
        }
    }
}
