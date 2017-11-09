using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _700IQ
{
    public class Questions
    {
        public string[] quest = new string[12];
        public string[] answer = new string[12];
    }
    public class  Game
    {
        public byte zoneUID;            //Индентификатор игровой зоны
        public byte iCon;               //номер Айкона
        public byte step;               //шаг в пределах айкона
        public byte Cell;               //Номер ячейки выпавшей на рулетке
        //public byte o1;                 //очередность ответов
        //public byte o2;                 //очередность ответов   
        //public byte o3;                 //очередность ответов
        public byte activeTable;        //номер активного стола
        public byte theme;              //тема вопроса
        public string quest;            //Текст вопроса      
        public string media;            //Медиаданные
        public Teames[] team = new Teames[3];
        public class Teames
        {
            public int uid;         // индентификатор команды  ????????????????????????                 
            public byte table;       //номер стола
            public int iQash;        //количество айкэш
            public int stavka;       //ставка команды
            public byte answerOrder; //очередность ответа команды
            public string answer;    //ответ на вопрос
            public bool correct;     //правильность ответа       
          
        }
    }
    public class Data//структура установочных данных
    {
        public DateTime startTime;  //начало игры
        public byte GameZone;       //игровая зона
        public string city;         //город
        public int idGame;          //id игры
        public string NameGame;     //название игры
        public int NumberGame;      //номер игры
       // public int NumberTable;     //номер стола
        public string Tur;          //тур
        public Temy[] tema = new Temy[7];   //темы на игру
        public teams[] team = new teams[3]; //команды на игру
        public class Temy
        {
            public byte themeId;
            public string theme;
 //           public byte themeColor;
        }
        public class teams
        {
           
            public int uid;         // индентификатор команды
            public string name;     //название команды
            public byte table;      //номер стола
            public int rating;      //рейтинг команды
            public int iQash;
            public string kod;      //ключ сессии
            public members[] member = new members[5]; //члены команды
            public bool Resumption;
            public class members
            {
                public int id;//идентификатор участика
                public string F; //фамилия
                public string N; //имя
                public int rait; //рейтнг
                public int dr;   //год рождения
            }
        }

    }
}
