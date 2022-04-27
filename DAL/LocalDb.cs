using MVVMDemo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMDemo.DAL
{
    public class LocalDb
    {
        private List<Student> students;

        public LocalDb()
        {
            Init();
        }

        private void Init()
        {
            students = new List<Student>();
            for (int i = 0; i < 30; i++)
            {
                students.Add(new Student()
                {
                    Id = i,
                    Name = string.Format("学生{0}", i),
                    Age = new Random(i).Next(0, 100),
                    Classes = i%2 ==0?"一班":"二班"
                });
            }
        }

        public List<Student> Query()
        {
            return students;
        }

        public List<Student> QueryByName(string name)
        {
            return students.Where((t)=>t.Name.Contains(name)).ToList();
        }

        public Student QueryById(int Id)
        {
            var student = students.FirstOrDefault((t) => (t.Id == Id));
            if (student != null)
            {
                return new Student()
                {
                    Id = student.Id,
                    Name = student.Name,
                    Age = student.Age,
                    Classes = student.Classes
                };
            }
            return null;
        }

        public void AddStudent(Student student)
        {
            if (student != null)
            {
                students.Add(student);
            }
        }

        public void DelStudent(int Id)
        {
            var student = students.FirstOrDefault((t) => (t.Id == Id));
            if (students != null)
            {
                _ = students.Remove(student);
            }
        }
    }
}
