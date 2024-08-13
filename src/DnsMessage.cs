using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_dns_server.src
{
    public class DnsMessage
    {
        public byte[] MessageBytes { get; }
        public DnsMessage(byte[] MessageBytes) {
            this.MessageBytes = MessageBytes ?? throw new ArgumentNullException(nameof(MessageBytes));

            ParseHeader(MessageBytes);
        }
        public ushort ID { get; set; }
        public bool QR { get; set; }

        public byte OPCODE { get; set; }
        public bool AA { get; set; }
        public bool TC { get; set; }
        public bool RD {  get; set; }
        public bool RA {  get; set; }
        public byte  Z {  get; set; }
        public byte RCODE { get; set; }
        public byte QDCOUNT { get; set; }
        public byte ANCOUNT { get; set; }
        public byte NSCOUNT { get; set; }
        public byte ARCOUNT { get; set; }

        private void ParseHeader(byte[] MessageBytes)
        {
            if (MessageBytes == null) 
                throw new ArgumentNullException(nameof(MessageBytes));
            if (MessageBytes.Length < 12) 
                throw new ArgumentException(nameof(MessageBytes));

            //Message Bytes[0] = 0000 0001 ejemplo
            //message bytes[1] = 0000 0010 ejemplo
            //explicacion (ushort)(MessageBytes[0] << 8 == 0000 0000 0000 0000  => 0000 0000 0000 0001 => << 8
            // => 0000 0001 0000 0000 => | messagesBytes[1] => 0000 0001 0000 0010
            ID = (ushort)(MessageBytes[0] << 8 | MessageBytes[1]);
             
            //Masking para sacar el primer Bit. 0b significa que es binario
            QR = (MessageBytes[2] & 0b10000000) == 1;
            // la mascara devuelve un byte 0xxxx000. pero debe devolver en el rango 0000 xxxx. shifteamos 3 veces a la derecha
            OPCODE = (byte)((byte)(MessageBytes[2] & 0b01111000) >> 3);
            AA = (byte)(MessageBytes[2] & 0b00000100) == 1;
            TC = (byte)(MessageBytes[2] & 0b00000010) == 1;
            RD = (byte)(MessageBytes[2] & 0b00000001) == 1;
            RA = (byte)(MessageBytes[3] & 0b10000000) == 1;
            Z = (byte)((byte)(MessageBytes[3] & 0b01110000) >> 4);
            RCODE = (byte)(MessageBytes[3] & 0b00001111);
            QDCOUNT = (byte)(MessageBytes[4] << 8  | MessageBytes[5]);
            ANCOUNT = (byte)(MessageBytes[6] << 8 | MessageBytes[7]);
            NSCOUNT = (byte)(MessageBytes[8] << 8 | MessageBytes[9]);
            ARCOUNT = (byte)(MessageBytes[10] << 8 | MessageBytes[11]);
        }

        public byte[] GetResponse()
        {
            var Response = new byte[12];

            Response[0] = (byte)(ID << 8);
            Response[1] = (byte)(ID & 0b11111111);
            Response[2] = (byte)(
                1 << 7 |
                OPCODE << 3 |
                (AA ? 1 : 0) << 2 |
                (TC ? 1 : 0) << 1 |
                (RD ? 1 : 0)
                );

            Response[3] = (byte)(
                (RA ? 1 : 0) << 7 |
                Z << 4 |
                (RCODE & 0b00001111)
                );

            Response[4] = (byte)(QDCOUNT >> 8);
            Response[5] = (byte)(QDCOUNT & 0b11111111);
            Response[6] = (byte)(ANCOUNT >> 8);
            Response[7] = (byte)(ANCOUNT & 0b11111111);
            Response[8] = (byte)(NSCOUNT >> 8);
            Response[9] = (byte)(NSCOUNT & 0b11111111);
            Response[10] = (byte)(ARCOUNT >> 8);
            Response[11] = (byte)(ARCOUNT & 0b11111111);

            return Response;
        }
    }
}
