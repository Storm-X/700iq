using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;
using System.Net;

namespace MainServer
{
    public class Questions
    {
        public string [] quest = new string[12];
        public string [] answer = new string[12];
    }

    public class Game
    {
        public byte zoneUID;            //Индентификатор игровой зоны
        public byte iCon;               //номер Айкона
        public byte step;               //шаг в пределах айкона
        public int Cell;                //Номер ячейки выпавшей на рулетке
        //public byte o1;                 //очередность ответов
        //public byte o2;                 //очередность ответов   
        //public byte o3;                 //очередность ответов
        public byte activeTable;        //номер активного стола
        public int idQuest;             //id вопроса
        public byte theme;              //тема вопроса
        public string quest;            //Текст вопроса  
        public string rightAnswer;      // Ответ на вопрос
        public string media;            //Медиаданные
        public Teames[] team = new Teames[3];
        public class Teames
        {
            
            public int uid;          // индентификатор команды  ????????????????????????                          
            public byte table;       //номер стола
            public int iQash;        //количество айкэш
            public int stavka;       //ставка команды
            public byte answerOrder; //очередность ответа команды
            public string answer;    //ответ на вопрос                                        
            public bool correct;     //правильность ответа            
        }
    }
    public class teams
    {    
        public int uid;         // индентификатор команды
        public string name;      //название команды
        public byte table;       //номер стола
        public int rating;      //рейтинг команды
        public int iQash;       //айкэш 
        public string kod;      //ключ сессии
        public bool Resumption;
        public members[] member = new members[5]; //члены команды
        public class members
        {
            public int id;//идентификатор участика
            public string F; //фамилия
            public string N; //имя
            public int rait; //рейтнг
            public int dr;   //год рождения
        }
    }
    public class Data//структура установочных данных
    {
        public DateTime startTime;  //начало игры
        public int GameZone;       //игровая зона
        public string city;         //город
        public int idGame;          //id игры
        public string NameGame;     //название игры
        public int NumberGame;      //номер игры
 //       public int NumberTable;     //номер стола

        public string Tur;          //тур
        public Temy[] tema = new Temy[7];   //темы на игру
        public teams[] team = new teams[3]; //команды на игру
        public class Temy
        {
            public byte themeId;
            public string theme;
            public string description;
            //           public string themeColor;
        }        
    }
    public struct ResiveData //структурированные данные получаемые сервером
    {
        public int uid;
        public string kluch;
        public byte table;
        public byte step;
        public string otvet;
        public int stavka;

    }
    
    public class SendLog
    {
        public Game gmLog;
        public Data dataLog;
        public int[] idTheme = new int[7];
        public string[] Themes = new string[7];
        public string usersid;
    }
    public class RND
    {
        public Random rand = new Random();
        private static object  block = new object();
        public int rnd()
        {
            lock (block)
            {
                return rand.Next(37);
            }
        }
    }
    public class Ruletka
    {
       
        public byte[] ResponsePriority(int indexCell, int[] Rates)
        {
            byte[] Roulet = { 0, 1, 3, 2, 1, 3, 2, 1, 2, 3, 1, 2, 1, 3, 2, 3, 1, 3, 2, 1, 2, 3, 1, 2, 1, 3, 2, 3, 1, 2, 3, 1, 3, 2, 1, 2, 3, 1, 3 };
            byte[] MaxSec = { 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 };

            if (indexCell == 0)
                indexCell++;
            string Sector = (MaxSec[indexCell] == 1) ? "| M" : "| ";
            Sector += Roulet[indexCell] + " | " + Roulet[indexCell + 1] + " | " + Roulet[indexCell + 2] + " |";
            //Console.WriteLine("Выбранный сектор на рулетке имеет следующий вид: {0}", Sector);
            try
            {
                Rates[Roulet[indexCell] - 1] += 1000 * (1 ^ MaxSec[indexCell++]) + 2;
                Rates[Roulet[indexCell] - 1]++;
                var dictRates = Enumerable.Range(0, Rates.Length).ToDictionary(x => ++x, x => Rates[x]).OrderByDescending(pair => pair.Value);
                return dictRates.Select(pair => (byte)pair.Key).ToArray();
            }
            catch
            {
                Console.WriteLine("Где-то косячок-с...");
            }
            return null;
        }       
    }
    public class RegData
    {
        public DataTable mem, kom;
        public DataSet dataSet;
        public bool canReg=false, gameStart=false;

        public void Set()
        {
            dataSet = new DataSet();
            kom = new DataTable("Teams");
            kom.Columns.Add("Zone",typeof(int)).Caption = "Игровая зона";//игровая зона - 0    
                                                //DataColumn dcID = 
            kom.Columns.Add("Id",typeof(int));//id команды - 1
            //dcID.Unique=true;
            //dt.PrimaryKey = new DataColumn[] { dt.Columns["Id"] };
            kom.Columns.Add("Name",typeof(string)).Caption = "Команда";//название команды - 2
            kom.Columns.Add("Key",typeof(string)).Caption = "Ключ";//ключ сессии - 3
            kom.Columns.Add("Rating", typeof(short)).Caption = "Рейтинг";    //рейтинг     -4              
            kom.Columns.Add("I-cash",typeof(int));//айкэш   -5  
            kom.Columns.Add("Table", typeof(int)).Caption = "Стол";//номер стола    - 6     
            kom.Columns.Add("Pause", typeof(bool)).Caption = "Пауза";//приостановка игры        
            dataSet.Tables.Add(kom);

            mem = new DataTable("Member");
            //DataColumn komID = 
            mem.Columns.Add("komID", typeof(int));
            mem.Columns.Add("id",typeof(int));// id -0
            mem.Columns.Add("N",typeof(string));// name -1           
            mem.Columns.Add("F",typeof(string));// family -2
            mem.Columns.Add("rating",typeof(short));//rating -3
            mem.Columns.Add("dr",typeof(string));//Birth Day - 4
            dataSet.Tables.Add(mem);

            //DataRelation drKomMember = new DataRelation("TeamMembers", dcID, komID);
            //dataSet.Relations.Add(drKomMember);
        }
        public DataTable ddt()
        {
            return  kom;
        }
        //public void addKom(int zona,int id, string nameK, short r, int iq)
        //{
        //    DataRow dtrow = kom.NewRow();
        //    dtrow[0] = zona;
        //    dtrow[1] = id;
        //    dtrow[2] = nameK;
        //    //dtrow[3] = IP;
        //    dtrow[4] = r;
        //    dtrow[5] = iq;
        //    kom.Rows.Add(dtrow);
        //}
        public void addMem (int idKom, int id, string N, string F, short r, int DR)
        {
            DataRow dtrow =mem.NewRow();
            dtrow[0] = idKom;
            dtrow[1] = id;
            dtrow[2] = N;
            dtrow[3] = F;
            dtrow[4] = r;
            dtrow[5] = DR;
            mem.Rows.Add(dtrow);
        }
        public DataTable get(int id)
        {
            DataRow[] rows = mem.Select("komID=" + id);
            DataTable dt = new DataTable();
            dt = mem.Clone();
            foreach (DataRow dr in rows)
            {
                object[] row = dr.ItemArray;
                dt.Rows.Add(row);
            }
            return dt;
        }
        public void deleteMem(int id)
        {
            DataRow[] member= mem.Select("komID="+id);
            foreach (DataRow dtmem in member)
            {
                dtmem.Delete();
            }
        }
        public bool getReg()
        {
            return canReg;
        }
        public bool getStart()
        {
            return gameStart;
        }
    }
    public class CheckOtvet
    {
        DataTable quest = new DataTable("quest");
        DataTable qu = new DataTable("Answer");     
      
        public void set()
        {
            qu.Columns.Add("Zona");
            qu.Columns.Add("Table");
            qu.Columns.Add("Id");
            qu.Columns.Add("Theme");
            qu.Columns.Add("Vopros");
            qu.Columns.Add("Otvet");
            qu.Columns.Add("KomOtvet");
            qu.Columns.Add("Правильно");
            qu.Columns.Add("Ok");
        }
        public DataTable get()
        {
            return qu;
        }
    }
   
}
