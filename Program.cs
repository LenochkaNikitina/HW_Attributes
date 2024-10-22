using System;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW_Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    class ParamsAttribute : Attribute
    {
        public string Filename {  get; set; }
        public ParamsAttribute(string filename)
        {
            Filename = filename;
        }
    }
        
    public class Student
    {
        [Params("name.ini")]
        public string Name { get; set; }

        [Params("age.ini")]
        public int Age { get; set; }

        [Params("group.ini")]
        public string Group { get; set; }

        public static Student CreateStudent()
        {
            var instance = new Student();
            
            foreach (var property in typeof(Student).GetProperties())
            {
                var prop = property.GetCustomAttribute<ParamsAttribute>();
                if (prop == null)
                    continue;

                var content = File.ReadAllText(prop.Filename);
                
                if(property.PropertyType == typeof(string))
                    property.SetValue(instance, content);
                else
                    property.SetValue(instance, Convert.ChangeType(content, property.PropertyType));
            }
            return instance; 
        }

        public override string ToString() { 
            var sb = new StringBuilder();
            sb.Append($"Name: {Name}");
            sb.Append($"Age: {Age}\n");
            sb.Append($"Group: {Group}");

            return sb.ToString();
        }
    }

    public static class Program
    {
        public static void Main()
        {
            /*
            using (StreamWriter file = new StreamWriter("age.ini"))
                file.WriteLine("21");
            using (StreamWriter file = new StreamWriter("name.ini"))
                file.WriteLine("Bruce Wayne");
            using (StreamWriter file = new StreamWriter("group.ini"))
                file.WriteLine("abc321");
            */
            Student student = Student.CreateStudent();
            Console.WriteLine(student);
            
            Console.ReadKey();
        }
    }
}
