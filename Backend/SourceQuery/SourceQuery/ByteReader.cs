using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Infrastructure.SourceQuery.Models.ServerInfo;

namespace Infrastructure.SourceQuery
{
    public class ByteReader
    {
        byte[] Response { get; set; }
        private int iterator = 0;

        public int Iterator
        {
            get
            {
                return iterator;
            }

            set
            {
                if(Response.Length > value && value >= 0)
                {
                    iterator = value;
                }
                else if(value >= Response.Length && value >= 0)
                {
                    iterator = -1;
                }
            }
        }

        public int Remaining
        {
            get
            {
                if(Iterator == -1)
                {
                    return 0;
                }

                return Response.Length - Iterator;
            }
        }

        public ByteReader(byte[] response)
        {
            Response = response;
        }

        public byte [] GetBytes()
        {
            return Response;
        }
        
        public byte GetByte()
        {
            if (Remaining < 1)
            {
                throw new ArgumentOutOfRangeException("Not Enough bytes left to read");
            }

            byte value = Response[Iterator];
            Iterator++;

            return value;
        }

        public int GetInt()
        {
            if (Remaining < sizeof(int))
            {
                throw new ArgumentOutOfRangeException("Not Enough bytes left to read");
            }

            int value = BitConverter.ToInt32(Response, Iterator);
            Iterator += sizeof(int);

            return value;
        }

        public uint GetLong()
        {
            if (Remaining < sizeof(uint))
            {
                throw new ArgumentOutOfRangeException("Not Enough bytes left to read");
            }

            uint value = BitConverter.ToUInt32(Response, Iterator);
            Iterator += sizeof(uint);

            return value;
        }

        public short GetShort()
        {
            if (Remaining < sizeof(short))
            {
                throw new ArgumentOutOfRangeException("Not Enough bytes left to read");
            }

            short value = BitConverter.ToInt16(Response, Iterator);
            Iterator += sizeof(short);

            return value;
        }

        public string GetString()
        {
            if (Remaining < 1)
            {
                throw new ArgumentOutOfRangeException("Not Enough bytes left to read");
            }

            int indexNextNullChar = Response.GetNextNullCharPosition(Iterator);

            if(indexNextNullChar == -1)
            {
                throw new ArgumentOutOfRangeException("No valid string could be found in the remaining bytes");
            }

            string value = Encoding.UTF8.GetString(Response, Iterator, indexNextNullChar - Iterator);
            Iterator = indexNextNullChar + 1;

            return value;
        }

        public float GetFloat()
        {
            if (Remaining < 4)
                throw new ArgumentOutOfRangeException("Not Enough bytes left to read");

            float floatValue = BitConverter.ToSingle(Response, Iterator);
            Iterator += 4;

            return floatValue;
        }

        /* private int GetNextNullCharIndex()
         {
             int index = Iterator;

             while (Response[index] != 0)
             {
                 index++;

                 if(Response.Length == index)
                 {
                     return -1;
                 }
             }

             return ++index;
         }*/

    }
}
