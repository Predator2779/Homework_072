using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework_07 
{
    struct Repository 
    {
        #region Переменные

        private Note[] notes;
        private string path;
        public int index;
        public string[] titles;

        #endregion

        /// <summary>
        /// Конструктор репозитория.
        /// </summary>
        /// <param name="Path">Путь к хранению</param>
        public Repository(string Path)
        {
            this.path = Path;
            this.index = 0;
            this.notes = new Note[0];
            this.titles = new string[0];
        }

        /// <summary>
        /// Меню ежедневника.
        /// </summary>
        public void MenuDiary()
        {
            Console.WriteLine("Добро пожаловать в ежедневник");

            DirectoryPath();
            bool dataIsEmpty = LoadData();

            char isEdit = 'y';

            while (isEdit == 'y')
            {
                Console.Clear();
                PrintDiary();

                if (!dataIsEmpty)
                {
                    Console.WriteLine("\nВыберите действие:\n\n" +
                                "[1] Создать запись.\n" +
                                "[2] Выбрать запись.\n" +
                                "[3] Поиск по диапазону дат.\n" +
                                "[4] Загрузить файл.\n" +
                                "[5] Сохранить файл.\n" +
                                "[6] Выйти.");

                    int region = Convert.ToInt32(Console.ReadLine());

                    switch (region)
                    {
                        case 1:
                            CreateNote();                            
                            break;
                        case 2:
                            SelectNote();
                            break;
                        case 3:
                            SearchByDate();
                            break;
                        case 4:
                            LoadData();
                            break;
                        case 5:
                            SaveData();
                            break;
                        default:
                            isEdit = 'n';
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Записей нет.\n" +
                            "Создать запись:\n");

                    CreateNote();
                    dataIsEmpty = false;
                }
            }                
        }

        /// <summary>
        /// Метод выбора записи.
        /// </summary>
        public void SelectNote()
        {
            Console.WriteLine("Выберите запись:");
            int index = Convert.ToInt32(Console.ReadLine());

            this.index = index - 1;
            Console.Clear();
            Console.WriteLine(this.notes[this.index].Print());

            Console.WriteLine("Выберите действие:\n" +
                        "[1] Изменить запись.\n" +
                        "[2] Удалить запись.");

            int region = Convert.ToInt32(Console.ReadLine());

            switch (region)
            {
                case 1:
                    EditNote();
                    break;
                case 2:
                    RemoveNote();
                    PrintDiary();
                    break;
            }
        }

        /// <summary>
        /// Метод создания записи.
        /// </summary>
        /// <param name="index">Индекс создаваемой записи</param>
        public void CreateNote()
        {
            this.notes = Resize(this.notes);
            this.index = this.notes.Length - 1;

            Console.WriteLine("Создание даты/времени заметки:");
            DateTime date = DateTime.Now;
            Console.WriteLine(date + "\n");
            Console.WriteLine("Создание дневного плана:");
            string planner = Convert.ToString(Console.ReadLine());
            Console.WriteLine("Создание списка покупок:");
            string purchases = Convert.ToString(Console.ReadLine());
            Console.WriteLine("Создание заметки:");
            string notation = Convert.ToString(Console.ReadLine());
            Console.WriteLine("Создание выпитого кол-ва воды:");
            string water = WaterIntToString(Convert.ToInt32(Console.ReadLine()));


            Console.Clear();
            Console.WriteLine(this.notes[this.index].Print());
            Add(this.index, new Note(date, planner, purchases, notation, water));               
        }

        /// <summary>
        /// Метод добавления записи в хранилище.
        /// </summary>
        /// <param name="ConcreteNote">Запись</param>
        public void Add(int index, Note ConcreteNote)
        {
            if (index >= this.notes.Length) this.notes = Resize(this.notes);
            this.notes[index] = ConcreteNote;
        }        

        /// <summary>
        /// Вывод в консоль записей вместе с заголовками.
        /// </summary>
        public void PrintDiary()
        {
            string[] titles = AddTitles();
            Console.WriteLine($"{titles[0],15} {titles[1],20} {titles[2],20} {titles[3],20} {titles[4],30}\n");

            int lengthNotes = this.notes.Length;

            for (int i = 0; i < lengthNotes; i++)
            {
                Console.Write($"[{i + 1}] ");
                Console.WriteLine(this.notes[i].Print());
            }
        }

        /// <summary>
        /// Метод указания директории.
        /// </summary>
        public void DirectoryPath()
        {
            bool existsDir = false;

            while (!existsDir)
            {
                Console.Write("Укажите диск на котором будет храниться файл: ");
                char disk = Convert.ToChar(Console.ReadLine().ToUpper());
                Console.Write(@"Укажите путь к папке в формате [my files\path]: ");
                string dir = Console.ReadLine();
                string path = disk + @":\" + dir;

                existsDir = Directory.Exists(path);

                if (existsDir)
                {
                    path += @"\data.txt";
                    this.path = path; Console.WriteLine("Выбранная папка: " + path + "\n");
                }
                else Console.WriteLine("Выбранная папка не найдена.\n");
            }

            if (!File.Exists(this.path))
            {
                this.notes = new Note[0];
                CreateNote();
                SaveData();
            }
        }

        /// <summary>
        /// Метод сохранения данных.
        /// </summary>
        public void SaveData()
        {
            File.WriteAllText(this.path, "");          /// Очистка файлв, перед записью.

            using (StreamWriter sw = new StreamWriter(this.path, true, Encoding.Unicode))
            {
                int notesLength = notes.Length;
                for (int i = 0; i < notesLength; i++)
                {
                    string line = 
                        $"{Convert.ToString(notes[i].Date),15}#" +
                        $"{notes[i].Planner}#" +
                        $"{notes[i].Purchases}#" +
                        $"{notes[i].Notation}#" +
                        $"{notes[i].Water}#";
                    
                    sw.WriteLine(line);
                }
            }

            Console.WriteLine("Сохранено.");
        }

        /// <summary>
        /// Метод загрузки данных. Проверяет есть ли данные.
        /// </summary>
        public bool LoadData()
        {  
            
            bool dataIsEmpty = DataIsEmpty();

            using (StreamReader sr = new StreamReader(this.path, Encoding.Unicode))
            {
                this.notes = new Note[0];
                this.notes = new Note[1];

                if (!dataIsEmpty)
                {
                    dataIsEmpty = false;

                    this.index = this.notes.Length - 1;

                    while (!sr.EndOfStream)
                    {
                        string[] args = sr.ReadLine().Split('#');

                        Add(this.index, new Note(Convert.ToDateTime(args[0]), args[1], args[2], args[3], args[4]));
                        index++;
                    }
                }
                else
                {
                    this.notes = new Note[0];
                    dataIsEmpty = true;
                }
            }

            return dataIsEmpty;
        }

        /// <summary>
        /// Метод проверки файла на наличие данных.
        /// </summary>
        /// <returns></returns>
        public bool DataIsEmpty()
        {
            bool isEmpty = true;

            using (StreamReader sr = new StreamReader(this.path, Encoding.Unicode))
            {
                if (sr.ReadLine() != String.Empty)
                    isEmpty = false;
                else
                    isEmpty = true;
            }

            return isEmpty;
        }

        /// <summary>
        /// Метод очистки массива записей.
        /// </summary>
        public void CleanNotes()
        {
            this.notes = new Note[0];
            this.notes = new Note[1];
            this.index = 0;
        }

        /// <summary>
        /// Метод поиска по диапазону дат.
        /// </summary>
        public void SearchByDate()
        {
            Console.WriteLine("Введите диапазон дат:");
            Console.WriteLine("С даты:");
            DateTime dateFrom = InputDateTime();
            Console.WriteLine("По дату:");
            DateTime dateBefore = InputDateTime();

            TimeSpan span = dateFrom - dateBefore;   /// Корректировка подсчета диапазона.
            if (Convert.ToInt32(span.TotalDays) > 0)
            {
                DateTime dt = dateFrom;
                dateFrom = dateBefore;
                dateBefore = dt;
            }

            string[] titles = AddTitles();
            Console.WriteLine($"{titles[0],15} {titles[1],20} {titles[2],20} {titles[3],20} {titles[4],30}\n");

            foreach (Note note in this.notes)
            {
                DateTime inputDate = Convert.ToDateTime(note.Date);
                if (inputDate >= dateFrom && inputDate <= dateBefore) Console.WriteLine(note.Print());
            }
            Console.WriteLine("\nПробел, чтобы продолжить");
            Console.ReadLine();
        }

        /// <summary>
        /// Метод изменения записи.
        /// </summary>
        /// <param name="index">Редактируемая запись</param>
        public void EditNote()
        {
            char isEdit = 'y';

            while (isEdit == 'y')
            {
                Console.WriteLine("Дописать:\n" +
                    "[1] Дневной план.\n" +
                    "[2] Заметки.\n" +
                    "[3] Покупки.\n" +
                    "[4] Количество воды.");

                int region = Convert.ToInt32(Console.ReadLine());

                switch (region)
                {
                    case 1:
                        notes[this.index].Planner += Console.ReadLine();
                        break;
                    case 2:
                        notes[this.index].Notation += Console.ReadLine();
                        break;
                    case 3:
                        notes[this.index].Purchases += Console.ReadLine();
                        break;
                    case 4:
                        notes[this.index].Water = WaterIntToString(Convert.ToInt32(Console.ReadLine()));
                        break;
                }

                Console.Clear();
                Console.WriteLine(this.notes[this.index].Print());
                Console.WriteLine("Продолжить редактирование? (y/n)");
                isEdit = Convert.ToChar(Console.ReadLine().ToLower());
            }
        }

        /// <summary>
        /// Метод удаления записи.
        /// </summary>
        public void RemoveNote()
        {
            int length = notes.Length;
            Note[] newNotes = new Note[length - 1];

            for (int i = 0; i < this.index; i++)
                newNotes[i] = this.notes[i];

            Console.WriteLine("len" + length + "ind" + this.index);

            for (int i = this.index + 1; i < length; i++)
                newNotes[i - 1] = this.notes[i];


            this.notes = newNotes;
        }

        /// <summary>
        /// Метод корректного ввода даты.
        /// </summary>
        /// <returns></returns>
        private DateTime InputDateTime()
        {
            DateTime date;
            while (true)
            {
                Console.WriteLine("Формат даты/времени: dd.mm.yyyy hh:mm:ss");
                try
                {
                    date = Convert.ToDateTime(Console.ReadLine());
                    break;
                }
                catch (System.FormatException)
                {
                    Console.WriteLine("Неверный формат даты/времени");                    
                }
            }
            return date;
        }

        /// <summary>
        /// Метод визуализации литров воды.
        /// </summary>
        /// <returns></returns>
        public string WaterIntToString(int water)
        {
            string waterVis = "";

            for (int i = 0; i < water; i++) waterVis += "@ ";

            return waterVis;
        }

        /// <summary>
        /// Метод возвращающий заголовки записей.
        /// </summary>
        /// <returns></returns>
        private string[] AddTitles()
            {
                string str = "Дата/время:,План на день:,Покупки:,Заметки:,Выпито литров воды:";
                string[] titles = str.Split(',');
        
                return titles;
            }
        
        /// <summary>
        /// Метод увеличения текущего хранилища на один.
        /// </summary>
        /// <param name="Flag">Условие увеличения</param>
        private Note[] Resize(Note[] notes)
        {
            Array.Resize(ref notes, notes.Length + 1);
            return notes;
        }
    }
}
