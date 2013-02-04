using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace irrparser
{
    class Advert
    {
        private String header;
        private String price;
        private String phone;
        private Boolean agent = false;

        public Advert(String header, String phone, String price, Boolean agent)
        {
            this.header = header;
            this.phone = phone;
            this.price = price;
            this.agent = agent;
        }

        public Advert() { }

        public Boolean IsAgent()
        {
            return agent;
        }

        public void SetAgent(Boolean agent)
        {
            this.agent = agent;
        }
        
        public String getHeader()
        {
            return header;
        }

        public String getPrice()
        {
            return price;
        }

        public String getPhone()
        {
            return phone;
        }

        public void setHeader(String header)
        {
            this.header = header;
        }

        public void setPrice(String price)
        {
            this.price = price;
        }

        public void setPhone(String phone)
        {
            this.phone = phone;
        }

        public String MakeString()
        {
            return this.getHeader() + "   " + this.getPrice() + "   " + this.getPhone();
        }
    }
}
