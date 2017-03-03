using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainServer
{
    class questionEditor
    {
        List<question> questionList;
        void Add(int id, string themes, string text, string answer)
        {
            question qu = new question(id,themes,text,answer);
            questionList.Add(qu); 
        }
        
    }

    class question
    {
        public int id;
        public string themes;
        public string text;
        public  string answer;
        public question(int id, string themes, string text, string answer)
        {
            this.id = id;
            this.themes = themes;
            this.text = text;
            this.answer = answer;
        }


    }
}
