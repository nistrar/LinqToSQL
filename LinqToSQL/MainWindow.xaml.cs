using System;
using System.Collections.Generic;
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
using System.Configuration;

namespace LinqToSQL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LinqToSQLDataClassesDataContext dataContext;
        public MainWindow()
        {
            
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings["LinqToSQL.Properties.Settings.linqDBConnectionString"].ConnectionString;

            dataContext = new LinqToSQLDataClassesDataContext(connectionString);
            //dataContext.ExecuteCommand("delete from student");

            //InsertUniversities();
            //InsertStudents();
            //InsertLectures();
            //InsertStudentLectureAssociations();
            //GetUniversityofDan();

            //GetLecturesFromDan();
            //GetAllLecturesFromBeijing();
            //UpdateDan();
            DeleteEma();
        }

        public void InsertUniversities()
        {
            dataContext.ExecuteCommand("delete from university");
            
            University yale = new University();
            yale.Name = "Yale";
            dataContext.Universities.InsertOnSubmit(yale);

            University beijing = new University();
            beijing.Name = "Beijing";
            dataContext.Universities.InsertOnSubmit(beijing);


            dataContext.SubmitChanges();

            MainDataGrid.ItemsSource = dataContext.Universities;
        }

        public void InsertStudents()
        {
            University yale = dataContext.Universities.First(un => un.Name.Equals("Yale"));
            University beijing = dataContext.Universities.First(un => un.Name.Equals("Beijing"));

            List<Student> students = new List<Student>();

            students.Add(new Student { Name = "Vlad", Gender = "male", UniversityID = yale.Id });
            students.Add(new Student { Name = "Dan", Gender = "male", UniversityID = yale.Id });
            students.Add(new Student { Name = "Ema", Gender = "female", UniversityID = beijing.Id });

            dataContext.Students.InsertAllOnSubmit(students);

            dataContext.SubmitChanges();
            
            MainDataGrid.ItemsSource = dataContext.Students;
        }

        public void InsertLectures()
        {
            List<Lecture> lectures = new List<Lecture>();

            lectures.Add(new Lecture { Name = "Mathematics" });
            lectures.Add(new Lecture { Name = "Thermodynamics" });
            lectures.Add(new Lecture { Name = "Information Technology" });

            dataContext.Lectures.InsertAllOnSubmit(lectures);

            dataContext.SubmitChanges();
        }

        public void InsertStudentLectureAssociations()
        {
            Student vlad = dataContext.Students.First(s => s.Name.Equals("Vlad"));
            Student dan = dataContext.Students.First(s => s.Name.Equals("Dan"));
            Student ema = dataContext.Students.First(s => s.Name.Equals("Ema"));

            Lecture math = dataContext.Lectures.First(l => l.Name.Equals("Mathematics"));
            Lecture thermo = dataContext.Lectures.First(l => l.Name.Equals("Thermodynamics"));
            Lecture informatics = dataContext.Lectures.First(l => l.Name.Equals("Information Technology"));

            dataContext.StudentLectures.InsertOnSubmit(new StudentLecture { Student = vlad, Lecture = math });
            dataContext.StudentLectures.InsertOnSubmit(new StudentLecture { Student = dan, Lecture = thermo });

            StudentLecture sEma = new StudentLecture();
            sEma.StudentID = ema.Id;
            sEma.LectureID = informatics.Id;
            dataContext.StudentLectures.InsertOnSubmit(sEma);

            dataContext.SubmitChanges();


            MainDataGrid.ItemsSource = dataContext.StudentLectures;

        }

        public void GetUniversityofDan()
        {
            Student dan = dataContext.Students.First(s => s.Name.Equals("Dan"));
            University dansUni = dan.University;

            List<University> universities = new List<University>();

            universities.Add(dansUni);
            
            MainDataGrid.ItemsSource = universities;

        }

        public void GetLecturesFromDan()
        {
            Student dan = dataContext.Students.First(s => s.Name.Equals("Dan"));

            var dansLectures = from sl in dan.StudentLectures select sl.Lecture;

            MainDataGrid.ItemsSource = dansLectures;
        }
        
        public void GetAllLecturesFromBeijing()
        {
            var lecturesFromBeijing = from sl in dataContext.StudentLectures
                                      join student in dataContext.Students
                                      on sl.StudentID equals student.Id
                                      where student.University.Name == "Yale"
                                      select sl.Lecture;

            MainDataGrid.ItemsSource = lecturesFromBeijing;
        }

        public void UpdateDan()
        {
            Student dan = dataContext.Students.FirstOrDefault(s => s.Name.Equals("Dan"));
            dan.Name = "Costea";
            dataContext.SubmitChanges();

            MainDataGrid.ItemsSource = dataContext.Students;
        }

        public void DeleteEma()
        {
            Student ema = dataContext.Students.FirstOrDefault(s => s.Name.Equals("Ema"));

            dataContext.Students.DeleteOnSubmit(ema);

            dataContext.SubmitChanges();

            MainDataGrid.ItemsSource = dataContext.Students;

        }
    }
}
