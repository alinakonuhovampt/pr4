using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            if (File.Exists("note.json"))
            newLists = JsonConvert.DeserializeObject<List<Note>>(File.ReadAllText("note.json"));
            if (File.Exists("notetype.json"))
                type = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText("notetype.json"));
            datagrid1.ItemsSource = newLists;
            dropdownmenu.ItemsSource = type;
            

            //Note Notee = new Note("name123", 33, new DateTime(), "1324");
            //string json = JsonConvert.SerializeObject(Notee);
            //itog.Text = json;
            //Note noteee = JsonConvert.DeserializeObject<Note>(json);
            //itog.Text = noteee.Name;
        }

        public class Note
        {
            public string Name { get; set; }
            public int summ;
            public int Amount => Math.Abs(summ);
            public bool income  => summ > 0; 
            public DateTime date;
            public string type { get; set; }


            public Note(string name, int summ, DateTime date, string type)
            {
                Name = name;
                this.summ = summ;
                this.date = date;
                this.type = type;
            }
        }
        public List<String> type = new List<String>();

        List<Note> newLists = new List<Note>();
        IEnumerable<Note> filterList => newLists.Where(note => note.date == Convert.ToDateTime(datepicker.Text));
        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            if (dropdownmenu.SelectedValue == null)
                return;
            newLists.Add(new Note (textboxname.Text, int.Parse(summ.Text),Convert.ToDateTime(datepicker.Text),(string)dropdownmenu.SelectedValue));

            countsumm();

            datagrid1.ItemsSource = filterList;
        }


        private void datepicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            countsumm();
            datagrid1.ItemsSource = filterList;
        }

        private void countsumm()
        {
            int s = 0;
            foreach (Note note in filterList)
                //newLists ес что тоже можно для всего
                s +=note.summ;
            itog.Text = "Итого: " + s; 
        }

        private void datagrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datagrid1.SelectedValue == null)
                return;
            textboxname.Text = ((Note)datagrid1.SelectedValue).Name;
            summ.Text = ((Note)datagrid1.SelectedValue).summ.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           if (datagrid1.SelectedValue == null)
               return;
           ((Note)datagrid1.SelectedValue).Name = textboxname.Text;
           ((Note)datagrid1.SelectedValue).summ = int.Parse(summ.Text);
           datagrid1.ItemsSource = filterList;

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (datagrid1.SelectedValue == null)
                return;
            newLists.Remove((Note)datagrid1.SelectedValue);
            datagrid1.ItemsSource = filterList;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Window1 window = new Window1();
            window.Show();
            window.Owner = Window.GetWindow(this);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
            string json = JsonConvert.SerializeObject(newLists);
            File.WriteAllText("note.json", json);
            json = JsonConvert.SerializeObject(type);
            File.WriteAllText("notetype.json", json);
        }
    }
}
