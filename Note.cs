using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework_07 
{
    struct Note
    {
        #region Конструктор.

        /// <summary>
        /// Добавление записи в хранилище.
        /// </summary>
        /// <param name="Date">Дата/время</param>
        /// <param name="Planner">Дневной план</param>
        /// <param name="Purchases">Список покупок</param>
        /// <param name="Notation">Заметки</param>
        /// <param name="Water">Выпито воды</param>
        public Note (DateTime Date, string Planner, string Purchases, string Notation, string Water)
        {
            this.date = Date;
            this.planner = Planner;
            this.purchases = Purchases;
            this.notation = Notation;
            this.water = Water;
        }

        #endregion

        #region Методы

        /// <summary>
        /// Метод вывода записей в консоль.
        /// </summary>
        /// <returns></returns>
        public string Print()
        {
            return $"{this.date,15} {this.planner,15} {this.purchases,20} {this.notation,25} {this.water,20}";
        }        

        #endregion

        #region Свойства

        /// <summary>
        /// Получение/добавление даты записи.
        /// </summary>
        public DateTime Date { get { return this.date; } set { this.date = value; } }

        /// <summary>
        /// Получение/добавление дневного плана.
        /// </summary>
        public string Planner { get { return this.planner; } set { this.planner = value; } }

        /// <summary>
        /// Получение/добавление заметки.
        /// </summary>л
        public string Notation { get { return this.notation; } set { this.notation = value; } }

        /// <summary>
        /// Получение/добавление покупок.
        /// </summary>
        public string Purchases { get { return this.purchases; } set { this.purchases = value; } }

        /// <summary>
        /// Получение/добавление информации о воде.
        /// </summary>
        public string Water { get { return this.water; } set { this.water = value; } }

        #endregion

        #region Поля

        /// <summary>
        /// Дата записи.
        /// </summary>
        private DateTime date;
        
        /// <summary>
        /// План на день.
        /// </summary>
        private string planner;

        /// <summary>
        /// Покупки.
        /// </summary>
        private string purchases;

        /// <summary>
        /// Для заметок.
        /// </summary>
        private string notation;

        /// <summary>
        /// Выпито литров воды за день.
        /// </summary>
        private string water;

        #endregion
    }
}