using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLC_Connection
{
    public interface PLC_Connection
    {
        int OPEN();
        int CLOSE();
        int ReadBlock();
        int WriteBlock();
        int Read();
        int Write();
        int isEmpty();


    }
    public class MX_com : PLC_Connection
    {
        public int CLOSE()
        {
            throw new NotImplementedException();
        }

        public int isEmpty()
        {
            throw new NotImplementedException();
        }

        public int OPEN()
        {
            throw new NotImplementedException();
        }

        public int Read()
        {
            throw new NotImplementedException();
        }

        public int ReadBlock()
        {
            throw new NotImplementedException();
        }

        public int Write()
        {
            throw new NotImplementedException();
        }

        public int WriteBlock()
        {
            throw new NotImplementedException();
        }
    }
    public class MitPLCPro : PLC_Connection
    {
        public int CLOSE()
        {
            throw new NotImplementedException();
        }

        public int isEmpty()
        {
            throw new NotImplementedException();
        }

        public int OPEN()
        {
            throw new NotImplementedException();
        }

        public int Read()
        {
            throw new NotImplementedException();
        }

        public int ReadBlock()
        {
            throw new NotImplementedException();
        }

        public int Write()
        {
            throw new NotImplementedException();
        }

        public int WriteBlock()
        {
            throw new NotImplementedException();
        }
    }
}
