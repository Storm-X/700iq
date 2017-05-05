using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace MainServer
{
   public class Ratings
    {
        GameinZone gz;
        MySqlConnection mycon;
        List<string> gameLogStr;
        List<SendLog> gameLog;
        double[] correct_answer = new double[3];
        double[] answer = new double[3];
        double[] Kz = new double[3];
        double[] Ks = new double[3];
        double[] Km = new double[3];
        double[] Rn = new double[3];
        double[] Rk = new double[3];
        double[] K = new double[3];
        int[] deltaK = new int[3];
        public Ratings(GameinZone gz, MySqlConnection mycon,int [] mesta)
        {
            this.gz = gz;
            this.mycon = mycon;
            for (int i = 0; i<3; i++)
            {
                correct_answer[i] = 0;
                answer[i] = 0;
            }
            for (int i = 0; i < 3; i++)
            {
                switch (mesta[i])
                {
                    case 1: Km[i] = 1; break;
                    case 2: Km[i] = 0.5; break;
                    case 3: Km[i] = 0.1; break;
                }
            }

        }
        public void ChangeRatings()
        {

         
        }
        public int[] getRatings()
        {
            gameLog = new List<SendLog>();
            string sql = "select command from logs where zone=" + gz.data.GameZone + " and gameid=" + gz.data.idGame;
            MySqlCommand cm = new MySqlCommand(sql, mycon);
            MySqlDataReader rd = cm.ExecuteReader();
            DataTable tournaments = new DataTable();
            using (rd)  //если есть данные, то записываем в таблицу dat
            {
                if (rd.HasRows) tournaments.Load(rd);
            }
            gameLogStr = new List<string>(tournaments.AsEnumerable().Select(r => r.Field<string>("command")).ToArray());
            foreach (String str in gameLogStr)
            {
                gameLog.Add(JsonConvert.DeserializeObject<SendLog>(str));
            }

            for (int i = 0; i < gameLog.Count; i++)
            {

                    if (gameLog[i].gmLog.team[2].answer != "")
                    {
                        answer[2]++;
                    }
                    if (gameLog[i].gmLog.team[1].answer != "")
                    {
                        answer[1]++;
                    }
                    if (gameLog[i].gmLog.team[0].answer != "")
                    {
                        answer[0]++;
                    }                  
                
             }

            for (int i = 0; i < gameLog.Count; i++)
            {
                int o1 = gameLog[i].gmLog.o1;
                int o2 = gameLog[i].gmLog.o2;
                int o3 = gameLog[i].gmLog.o3;

                if ((gameLog[i].gmLog.team[o1-1].correct)|| (gameLog[i].gmLog.team[o2-1].correct)||(gameLog[i].gmLog.team[o3-1].correct))
                {
                    if (String.Compare(gameLog[i].gmLog.team[o3-1].answer,"") != 0 && String.Compare(gameLog[i].gmLog.team[o3-1].answer,"Нет ответа")!=0)
                    {
                        correct_answer[o3 - 1]++;
                        continue;
                    }
                    if (String.Compare(gameLog[i].gmLog.team[o2-1].answer,"")!=0 && String.Compare(gameLog[i].gmLog.team[o2 - 1].answer,"Нет ответа")!=0 && String.Compare(gameLog[i].gmLog.team[o3-1].answer,"")==0)
                    {
                        correct_answer[o2 - 1]++;
                        continue;
                    }
                    if (String.Compare(gameLog[i].gmLog.team[o1-1].answer,"")!=0 && String.Compare(gameLog[i].gmLog.team[o1-1].answer,"Нет ответа")!=0 && String.Compare(gameLog[i].gmLog.team[o2-1].answer,"")==0)
                    {
                        correct_answer[o1 - 1]++;
                        continue;
                    }
                }
            }
            double sumiQash = gameLog[gameLog.Count - 1].gmLog.team[0].iQash + gameLog[gameLog.Count - 1].gmLog.team[1].iQash + gameLog[gameLog.Count - 1].gmLog.team[2].iQash;
            double sumRating = gameLog[gameLog.Count - 1].dataLog.team[0].rating + gameLog[gameLog.Count - 1].dataLog.team[1].rating + gameLog[gameLog.Count - 1].dataLog.team[2].rating;
           /* Km[0] = 1;
            Km[1] = 0.5;
            Km[2] = 0.1;*/

            for (int i=0;i<3;i++)
            {
                Kz[i] = 1 + 2 * (correct_answer[i]/answer[i]*(12+correct_answer[i])/24);
                Ks[i] = 1 + gameLog[gameLog.Count - 1].gmLog.team[i].iQash / sumiQash;
                Rn[i] = 50 * gameLog[gameLog.Count - 1].dataLog.team[i].rating / sumRating;
                K[i] = Kz[i] * Ks[i] * Km[i];
            }
            double sumK = K[0] + K[1] + K[2];
            for (int i = 0; i < 3; i++)
            {
                Rk[i] = 50 * K[i] / sumK;
                deltaK[i] = (int)Rk[i] - (int)Rn[i];
            }

            return deltaK;
        }
    }
}
