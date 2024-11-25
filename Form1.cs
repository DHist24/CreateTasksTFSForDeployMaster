using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.IO;
using SeleniumKeys = OpenQA.Selenium.Keys;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Numerics;
using System.Text.RegularExpressions;


//Условие Создать Task должно быть выведено в закрепленных
//16 09 24 Работа со списком тасков
//17 09 24 Работа с TXT файлом
//19 09 24 Удаляем из txt лишние пробелы
//23 09 24 Удаление одинакового кода








namespace CreateTasksTFS
{
    public partial class Form1 : Form
    {
        IWebDriver Browser;


        private readonly By _Create = By.CssSelector(".navigation-icon.flex-row.flex-center.justify-center.flex-noshrink.fabric-icon.ms-Icon--Add.medium");
        private readonly By _Activity = By.CssSelector("[aria-label=\"Activity\"]");
        private readonly By _CreateTask = By.CssSelector("#__bolt-new-workitem-task");
        private readonly By _Title = By.CssSelector("[aria-label=\"Поле заголовка\"]");
        private readonly By _Sprint = By.CssSelector("[aria-label=\"Iteration Path\"]");
        private readonly By _SprintText = By.XPath("//input[@class='tree-picker-input']");


        private readonly By _AssignedTo = By.CssSelector("[class*=\"identity-picker-watermark-name text-cursor\"]");
        private readonly By _FindAssignedTo = By.CssSelector("[aria-label=\"Поиск среди пользователей\"]");
        private readonly By _LinkButton = By.CssSelector("[aria-label=\"Links\"]");
        private readonly By _dialogWin = By.CssSelector("div.ui-dialog.ui-widget.ui-widget-content.ui-corner-all.ui-front.ui-dialog-buttons.ui-draggable.ui-resizable");
        private readonly By _TypeOfConnect = By.CssSelector("[class*=\"ms-ComboBox-Input css-\"]");
        private readonly By _IDTitle = By.Id("work-item-ids");
        private readonly By _OK = By.Id("ok");
        private readonly By _SaveButton = By.XPath("//span[@role='button'][contains(text(),'Сохранить и закрыть')]");
        string Tester;
        string TesterPlan;
        string TesterTXT;

        int StringToIntNumberOfTasks;
        string NumberOfTasks;





        public Form1()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;


            textBoxTester1.ReadOnly = true;
            textBoxTester2.ReadOnly = true;
            textBoxTester3.ReadOnly = true;
            textBoxTester4.ReadOnly = true;




        }

        public void Form1_Load(object sender, EventArgs e)
        {



        }



        private void OkInParent()
        {
            WebDriverWait wait = new WebDriverWait(Browser, TimeSpan.FromSeconds(10));
            IWebElement dialogWin = Browser.FindElement(_dialogWin);
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(_OK));
            IWebElement OK = dialogWin.FindElement(_OK);
            OK.Click();
        }


        private void ParentDeploy()
        {
            var ParentID = textBox2.Text;

            WebDriverWait wait = new WebDriverWait(Browser, TimeSpan.FromSeconds(10));

            ParentMain();

            IWebElement dialogWin = Browser.FindElement(_dialogWin);
            IWebElement IDTitle = dialogWin.FindElement(_IDTitle);//Поле ID

            IDTitle.SendKeys(ParentID);
            IDTitle.Equals(ParentID);
            Thread.Sleep(1000);
            IDTitle.SendKeys(SeleniumKeys.Enter);

            OkInParent();

        }

        public void ParentPlan()
        {
            var ParentIDPlan = textBoxIDPlan.Text;

            WebDriverWait wait = new WebDriverWait(Browser, TimeSpan.FromSeconds(10));

            ParentMain();

            IWebElement dialogWin = Browser.FindElement(_dialogWin);
            IWebElement IDTitle = dialogWin.FindElement(_IDTitle);//Поле ID

            IDTitle.SendKeys(ParentIDPlan);
            IDTitle.Equals(ParentIDPlan);
            Thread.Sleep(1000);
            IDTitle.SendKeys(SeleniumKeys.Enter);

            OkInParent();

        }

        public void ParentTXT()
        {

            var ParentIDTXT = textBoxIDTXT.Text;
            WebDriverWait wait = new WebDriverWait(Browser, TimeSpan.FromSeconds(10));

            ParentMain();

            IWebElement dialogWin = Browser.FindElement(_dialogWin);
            IWebElement IDTitle = dialogWin.FindElement(_IDTitle);//Поле ID
            IDTitle.SendKeys(ParentIDTXT);
            IDTitle.Equals(ParentIDTXT);
            Thread.Sleep(1000);
            IDTitle.SendKeys(SeleniumKeys.Enter);

            OkInParent();

        }

        private void ParentMain()
        {
            WebDriverWait wait = new WebDriverWait(Browser, TimeSpan.FromSeconds(10));

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(_LinkButton));//кнопка "цепочка"
            IWebElement LinkButton = Browser.FindElement(_LinkButton);
            LinkButton.Click();


            IWebElement AddLinkButton = Browser.FindElement(By.CssSelector("div.dropdown-button-wrapper")); //кнопка Добавить ссылку    
            AddLinkButton.Click();


            IWebElement MenuLinks = Browser.FindElement(By.Name("Существующий элемент")); //кнопка Существующий элемент
            MenuLinks.Click();

            //Диалоговое окно привязки элемента
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(_dialogWin));
            IWebElement dialogWin = Browser.FindElement(_dialogWin);


            IWebElement TypeOfConnect = dialogWin.FindElement(_TypeOfConnect); //parent динамическая ячейка 
            TypeOfConnect.Click();
            TypeOfConnect.SendKeys("Parent");
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementWithText(_TypeOfConnect, "Parent"));



            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(_IDTitle));
            IWebElement IDTitle = dialogWin.FindElement(_IDTitle);//Поле ID
            IDTitle.Click();


        }

        private void CreateTask() //до момента заполнения полей
        {
            WebDriverWait wait = new WebDriverWait(Browser, TimeSpan.FromSeconds(10));


            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(_Create)); //+
            IWebElement Create = Browser.FindElement(_Create);
            Create.Click();

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(_CreateTask));//Создать Task
            IWebElement CreateTask = Browser.FindElement(_CreateTask);
            CreateTask.Click();


            //переход на новую страницу


        }



        public void SelectUser()
        {


            if (textBoxTester1.ReadOnly == false) Tester = checkBoxTester1.Text;
            if (textBoxTester2.ReadOnly == false) Tester = checkBoxTester2.Text;
            if (textBoxTester3.ReadOnly == false) Tester = checkBoxTester3.Text;
            if (textBoxTester4.ReadOnly == false) Tester = checkBoxTester4.Text;

            WebDriverWait wait = new WebDriverWait(Browser, TimeSpan.FromSeconds(10));

            SelectUserMain();

            IWebElement FindAssignedTo = Browser.FindElement(_FindAssignedTo);
            FindAssignedTo.SendKeys(Tester);
            Thread.Sleep(1000);
            FindAssignedTo.SendKeys(SeleniumKeys.Enter);
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementWithText(_FindAssignedTo, Tester));




        }

        public void SelectUserPlan()
        {
            if (CheckBoxTesterPlan1.Enabled == true) TesterPlan = CheckBoxTesterPlan1.Text;
            if (CheckBoxTesterPlan2.Enabled == true) TesterPlan = CheckBoxTesterPlan2.Text;
            if (CheckBoxTesterPlan3.Enabled == true) TesterPlan = CheckBoxTesterPlan3.Text;
            if (CheckBoxTesterPlan4.Enabled == true) TesterPlan = CheckBoxTesterPlan4.Text;

            WebDriverWait wait = new WebDriverWait(Browser, TimeSpan.FromSeconds(10));

            SelectUserMain();


            IWebElement FindAssignedTo = Browser.FindElement(_FindAssignedTo);
            FindAssignedTo.SendKeys(TesterPlan);
            Thread.Sleep(1000);
            FindAssignedTo.SendKeys(SeleniumKeys.Enter);
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementWithText(_FindAssignedTo, TesterPlan));


        }

        public void SelectUserTXT()
        {
            if (CheckBoxTesterTXT1.Enabled == true) TesterTXT = CheckBoxTesterTXT1.Text;
            if (CheckBoxTesterTXT2.Enabled == true) TesterTXT = CheckBoxTesterTXT2.Text;
            if (CheckBoxTesterTXT3.Enabled == true) TesterTXT = CheckBoxTesterTXT3.Text;
            if (CheckBoxTesterTXT4.Enabled == true) TesterTXT = CheckBoxTesterTXT4.Text;

            WebDriverWait wait = new WebDriverWait(Browser, TimeSpan.FromSeconds(10));


            SelectUserMain();

            IWebElement FindAssignedTo = Browser.FindElement(_FindAssignedTo);
            FindAssignedTo.SendKeys(TesterTXT);
            Thread.Sleep(1000);
            FindAssignedTo.SendKeys(SeleniumKeys.Enter);
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementWithText(_FindAssignedTo, TesterTXT));
        }


        public void SelectUserMain()
        {
            WebDriverWait wait = new WebDriverWait(Browser, TimeSpan.FromSeconds(10));

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(_AssignedTo));   // Кому назначено
            IWebElement AssignedTo = Browser.FindElement(_AssignedTo);
            AssignedTo.Click();

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(_FindAssignedTo));   // поиск пользователя
            IWebElement FindAssignedTo = Browser.FindElement(_FindAssignedTo);
            FindAssignedTo.Click();


        }


        public void Sprint()
        {
            var Iteration = textBox4.Text;

            SprintMain();

            IWebElement SprintText = Browser.FindElement(_SprintText);
            SprintText.SendKeys(Iteration);
            Thread.Sleep(1000);
            SprintText.SendKeys(SeleniumKeys.Enter);
            SprintText.Equals(Iteration);



        }

        public void SprintPlan()
        {
            var IterationPlan = textBoxSprintPlan.Text;
            SprintMain();

            IWebElement SprintText = Browser.FindElement(_SprintText);
            SprintText.SendKeys(IterationPlan);
            Thread.Sleep(1000);
            SprintText.SendKeys(SeleniumKeys.Enter);
            SprintText.Equals(IterationPlan);

        }

        public void SprintTXT()
        {
            var IterationTXT = textBoxSprintTXT.Text;

            SprintMain();

            IWebElement SprintText = Browser.FindElement(_SprintText);
            SprintText.SendKeys(IterationTXT);
            Thread.Sleep(1000);
            SprintText.SendKeys(SeleniumKeys.Enter);
            SprintText.Equals(IterationTXT);

        }

        public void SprintMain()
        {

            WebDriverWait wait = new WebDriverWait(Browser, TimeSpan.FromSeconds(10));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(_Sprint));//спринт
            IWebElement Sprint = Browser.FindElement(_Sprint);
            Sprint.Click();

            IWebElement SprintText = Browser.FindElement(_SprintText);
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(_SprintText));//спринт
            SprintText.Click();
            SprintText.SendKeys(SeleniumKeys.LeftControl + "a");

        }

        public void Activity()
        {
            WebDriverWait wait = new WebDriverWait(Browser, TimeSpan.FromSeconds(10));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(_Activity));   // Activity
            IWebElement Activity = Browser.FindElement(_Activity);
            Activity.Click();
            Activity.SendKeys("Testing");
            Thread.Sleep(1000);
            Activity.SendKeys(SeleniumKeys.Enter);
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementWithText(_Activity, "Testing"));

        }


        public void NameTask()
        {

            var TitleTask = textBox1.Text;
            WebDriverWait wait = new WebDriverWait(Browser, TimeSpan.FromSeconds(10));

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(_Title));//заголовок таска
            IWebElement Title = Browser.FindElement(_Title);
            Title.Click();

            Title.SendKeys(TitleTask);
            Thread.Sleep(1000);
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementWithText(_Title, TitleTask));

        }

        public void SaveTask()
        {
            WebDriverWait wait = new WebDriverWait(Browser, TimeSpan.FromSeconds(10));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(_SaveButton));
            IWebElement SaveButton = Browser.FindElement(_SaveButton);
            SaveButton.Click();
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(_Create));
        }



        public void button1_Click(object sender, EventArgs e) // запуск браузера
        {
            OpenQA.Selenium.Chrome.ChromeOptions co = new OpenQA.Selenium.Chrome.ChromeOptions();
            co.AddArgument(@"user-data-dir=C:\Users\nataliya\AppData\Local\Google\Chrome\User Data\"); //переделай путь если  хочешь запустить

            Browser = new OpenQA.Selenium.Chrome.ChromeDriver(co); //открытие браузера с настройками
            Browser.Manage().Window.Maximize();                  //развернуть на весь экран

            Browser.Navigate().GoToUrl("http://iprv014");     //перейти по url 

        }

        public void button2_Click(object sender, EventArgs e) // Создать Task для Deploy
        {
            var ParentID = textBox2.Text;


            if (textBoxTester1.ReadOnly == false) { NumberOfTasks = textBoxTester1.Text; }
            if (textBoxTester2.ReadOnly == false) { NumberOfTasks = textBoxTester2.Text; }
            if (textBoxTester3.ReadOnly == false) { NumberOfTasks = textBoxTester3.Text; }
            if (textBoxTester4.ReadOnly == false) { NumberOfTasks = textBoxTester4.Text; }
            if (textBox3.ReadOnly == false) { NumberOfTasks = textBox3.Text; }

            StringToIntNumberOfTasks = Convert.ToInt32(NumberOfTasks);

            for (int i = 0; i < StringToIntNumberOfTasks; i++)
            {

                CreateTask();
                NameTask();
                Activity();
                Sprint();

                if (checkBoxTester1.Checked || checkBoxTester2.Checked || checkBoxTester3.Checked || checkBoxTester4.Checked) SelectUser();



                if (string.IsNullOrEmpty(ParentID) == false) // указание parent
                {
                    ParentDeploy();

                }


                SaveTask();



            }




        }


        private void buttonPlan_Click(object sender, EventArgs e) // Создать Task для Plan
        {

            var ParentIDPlan = textBoxIDPlan.Text;
            var TitleTaskPlan = new List<string>() { TextBox1Plan.Text, TextBox2Plan.Text, TextBox3Plan.Text, TextBox4Plan.Text, TextBox5Plan.Text };
            WebDriverWait wait = new WebDriverWait(Browser, TimeSpan.FromSeconds(10));




            for (int i = 0; i < TitleTaskPlan.Count; i++)
            {
                if (string.IsNullOrEmpty(TitleTaskPlan[i]) == false)
                {
                    CreateTask();


                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(_Title));//заголовок таска
                    IWebElement Title = Browser.FindElement(_Title);
                    Title.Click();
                    Title.SendKeys(TitleTaskPlan[i]);
                    Thread.Sleep(1000);
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementWithText(_Title, TitleTaskPlan[i]));


                    Activity();
                    SprintPlan();
                    SelectUserPlan();
                    if (string.IsNullOrEmpty(ParentIDPlan) == false) // указание parent
                    {
                        ParentPlan();

                    }

                    SaveTask();

                }
            }

        }


        private void button3_Click(object sender, EventArgs e) //Создание task из txt
        {
            var ParentIDTXT = textBoxIDTXT.Text;
            string StringTask;
            string StringWithoutSpace;

            WebDriverWait wait = new WebDriverWait(Browser, TimeSpan.FromSeconds(10));


            StreamReader f = new StreamReader("D:\\Tasks.txt");

            while ((StringTask = f.ReadLine()) != null)
            {

                CreateTask();

                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(_Title));//заголовок таска
                IWebElement Title = Browser.FindElement(_Title);
                Title.Click();
                StringWithoutSpace = Regex.Replace(StringTask, "[\t]+", " "); //удаляем лишние пробелы в txt
                Title.SendKeys(StringWithoutSpace);
                Thread.Sleep(1000);
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementWithText(_Title, StringWithoutSpace));

                Activity();
                SprintTXT();
                SelectUserTXT();
                if (string.IsNullOrEmpty(ParentIDTXT) == false) // указание parent
                {
                    ParentTXT();
                }

                SaveTask();
            }
            f.Close();



        }


        private void checkBoxTester1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxTester1.Checked == true)
            {

                textBox3.ReadOnly = true;

                checkBoxTester2.Enabled = false;
                checkBoxTester3.Enabled = false;
                checkBoxTester4.Enabled = false;

                textBoxTester1.ReadOnly = false;
                textBoxTester2.ReadOnly = true;
                textBoxTester3.ReadOnly = true;
                textBoxTester4.ReadOnly = true;

            }
            else
            {

                textBox3.ReadOnly = false;
                checkBoxTester2.Enabled = true;
                checkBoxTester3.Enabled = true;
                checkBoxTester4.Enabled = true;

                textBoxTester1.ReadOnly = true;
                textBoxTester2.ReadOnly = true;
                textBoxTester3.ReadOnly = true;
                textBoxTester4.ReadOnly = true;


            }

        }

        private void checkBoxTester2_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBoxTester2.Checked == true)
            {
                textBox3.ReadOnly = true;

                checkBoxTester1.Enabled = false;
                checkBoxTester3.Enabled = false;
                checkBoxTester4.Enabled = false;


                textBoxTester1.ReadOnly = true;
                textBoxTester2.ReadOnly = false;
                textBoxTester3.ReadOnly = true;
                textBoxTester4.ReadOnly = true;




            }
            else
            {
                textBox3.ReadOnly = false;

                checkBoxTester1.Enabled = true;
                checkBoxTester3.Enabled = true;
                checkBoxTester4.Enabled = true;

                textBoxTester1.ReadOnly = true;
                textBoxTester2.ReadOnly = true;
                textBoxTester3.ReadOnly = true;
                textBoxTester4.ReadOnly = true;




            }
        }

        private void checkBoxTester3_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBoxTester3.Checked == true)
            {
                textBox3.ReadOnly = true;

                checkBoxTester1.Enabled = false;
                checkBoxTester2.Enabled = false;
                checkBoxTester4.Enabled = false;


                textBoxTester1.ReadOnly = true;
                textBoxTester2.ReadOnly = true;
                textBoxTester3.ReadOnly = false;
                textBoxTester4.ReadOnly = true;




            }
            else
            {
                textBox3.ReadOnly = false;

                checkBoxTester1.Enabled = true;
                checkBoxTester2.Enabled = true;
                checkBoxTester4.Enabled = true;

                textBoxTester1.ReadOnly = true;
                textBoxTester2.ReadOnly = true;
                textBoxTester3.ReadOnly = true;
                textBoxTester4.ReadOnly = true;

            }
        }

        private void checkBoxTester4_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBoxTester4.Checked == true)
            {
                textBox3.ReadOnly = true;

                checkBoxTester1.Enabled = false;
                checkBoxTester2.Enabled = false;
                checkBoxTester3.Enabled = false;


                textBoxTester1.ReadOnly = true;
                textBoxTester2.ReadOnly = true;
                textBoxTester3.ReadOnly = true;
                textBoxTester4.ReadOnly = false;




            }
            else
            {
                textBox3.ReadOnly = false;

                checkBoxTester1.Enabled = true;
                checkBoxTester2.Enabled = true;
                checkBoxTester3.Enabled = true;

                textBoxTester1.ReadOnly = true;
                textBoxTester2.ReadOnly = true;
                textBoxTester3.ReadOnly = true;
                textBoxTester4.ReadOnly = true;
            }
        }

        private void CheckBoxTesterPlan1_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxTesterPlan1.Checked == true)
            {



                CheckBoxTesterPlan2.Enabled = false;
                CheckBoxTesterPlan3.Enabled = false;
                CheckBoxTesterPlan4.Enabled = false;



            }
            else
            {

                CheckBoxTesterPlan2.Enabled = true;
                CheckBoxTesterPlan3.Enabled = true;
                CheckBoxTesterPlan4.Enabled = true;


            }
        }

        private void CheckBoxTesterPlan2_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxTesterPlan2.Checked == true)
            {



                CheckBoxTesterPlan1.Enabled = false;
                CheckBoxTesterPlan3.Enabled = false;
                CheckBoxTesterPlan4.Enabled = false;



            }
            else
            {

                CheckBoxTesterPlan1.Enabled = true;
                CheckBoxTesterPlan3.Enabled = true;
                CheckBoxTesterPlan4.Enabled = true;


            }
        }

        private void CheckBoxTesterPlan3_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxTesterPlan3.Checked == true)
            {



                CheckBoxTesterPlan1.Enabled = false;
                CheckBoxTesterPlan2.Enabled = false;
                CheckBoxTesterPlan4.Enabled = false;



            }
            else
            {

                CheckBoxTesterPlan1.Enabled = true;
                CheckBoxTesterPlan2.Enabled = true;
                CheckBoxTesterPlan4.Enabled = true;


            }
        }

        private void CheckBoxTesterPlan4_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxTesterPlan4.Checked == true)
            {



                CheckBoxTesterPlan2.Enabled = false;
                CheckBoxTesterPlan3.Enabled = false;
                CheckBoxTesterPlan1.Enabled = false;



            }
            else
            {

                CheckBoxTesterPlan2.Enabled = true;
                CheckBoxTesterPlan3.Enabled = true;
                CheckBoxTesterPlan1.Enabled = true;


            }
        }

        private void CheckBoxTesterTXT1_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxTesterTXT1.Checked == true)
            {



                CheckBoxTesterTXT2.Enabled = false;
                CheckBoxTesterTXT3.Enabled = false;
                CheckBoxTesterTXT4.Enabled = false;



            }
            else
            {

                CheckBoxTesterTXT2.Enabled = true;
                CheckBoxTesterTXT3.Enabled = true;
                CheckBoxTesterTXT4.Enabled = true;


            }
        }

        private void CheckBoxTesterTXT2_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxTesterTXT2.Checked == true)
            {



                CheckBoxTesterTXT1.Enabled = false;
                CheckBoxTesterTXT3.Enabled = false;
                CheckBoxTesterTXT4.Enabled = false;



            }
            else
            {

                CheckBoxTesterTXT1.Enabled = true;
                CheckBoxTesterTXT3.Enabled = true;
                CheckBoxTesterTXT4.Enabled = true;


            }
        }

        private void CheckBoxTesterTXT3_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxTesterTXT3.Checked == true)
            {



                CheckBoxTesterTXT1.Enabled = false;
                CheckBoxTesterTXT2.Enabled = false;
                CheckBoxTesterTXT4.Enabled = false;



            }
            else
            {

                CheckBoxTesterTXT1.Enabled = true;
                CheckBoxTesterTXT2.Enabled = true;
                CheckBoxTesterTXT4.Enabled = true;


            }
        }

        private void CheckBoxTesterTXT4_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxTesterTXT4.Checked == true)
            {



                CheckBoxTesterTXT2.Enabled = false;
                CheckBoxTesterTXT3.Enabled = false;
                CheckBoxTesterTXT1.Enabled = false;



            }
            else
            {

                CheckBoxTesterTXT2.Enabled = true;
                CheckBoxTesterTXT3.Enabled = true;
                CheckBoxTesterTXT1.Enabled = true;


            }
        }
    }
}
