namespace CSH_Monitor.UI.Features.EntryWindow
{
    public partial class EntryWindowViewModel
    {
        private int selectedTabIndex;
        public int SelectedTabIndex
        {
            get => selectedTabIndex;
            set
            {
                if (selectedTabIndex != value)
                {
                    SetProperty(ref selectedTabIndex, value);
                    UpdateContentForSelectedTab();
                }
            }
        }

        private string currentDisplayText;
        public string CurrentDisplayText
        {
            get => currentDisplayText;
            set => SetProperty(ref currentDisplayText, value);
        }

        private void UpdateContentForSelectedTab()
        {
            // нужно будет раскидать логику по "вкладочным" партиалам позднее в виде void методов
            switch (selectedTabIndex)
            {
                case 0:
                    CurrentDisplayText = "Текст для первой вкладки";
                    break;
                case 1:
                    CurrentDisplayText = "Текст для второй вкладки";
                    break;
                case 2:
                    CurrentDisplayText = "Текст для третьей вкладки";
                    break;
                default:
                    CurrentDisplayText = "Критическая ошибка";
                    break;
            }
        }

    }
}