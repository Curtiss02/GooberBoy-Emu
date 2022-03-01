using System;

namespace GooberBoy_Emu.GameboyComponents
{
    class GBCPU
    {
        GBMemory _mem = new GBMemory();
        GBRegister AF;
        GBRegister BC;
        GBRegister DE;
        GBRegister HL;
        GBRegister SP;
        GBRegister PC;
        byte OP;

        public void ExecuteCycle()
        {
            fetch();
            execute();
        }
        void fetch()
        {
            OP = _mem.Read(PC.Value);
        }
        void execute()
        {
            bool branch = false;
            bool cb = false;
            Console.WriteLine("Executing OP: {1:x} ({2})", OP, OpcodeDetails.OpcodeNames[OP]);
            switch (OP)
            {
                case 0x00:
                    break;
                default:
                    Console.WriteLine("Unimplemted OP: {1:x} ({2})", OP, OpcodeDetails.OpcodeNames[OP]);
                    break;
            }
        }
        //------------------------------- ARITHMETIC FUNCTIONS -----------------------------//

        // HELPER FUNCTIONS
        void add_set_flags(byte a, byte b)
        {
            if(byte.MaxValue - a < b)
            {
                setCarryFlag(true);
            }
            else
            {
                setCarryFlag(false);
            }
            if((a & 0xf + b & 0xf & 0x10) > 0){
                setHalfCarryFlag(true);
            }
            else
            {
                setHalfCarryFlag((false));
            }

            if (a + b == 0)
            {
                setZeroFlag(true);
            }
            else
            {
                setZeroFlag(false);
            }
            setNegativeFlag(false);
        }
        void and_set_flags(byte a, byte b)
        {
            if ((a & b) == 0)
            {
                setZeroFlag(true);
            }
            else
            {
                setZeroFlag(false);
            }
            setNegativeFlag(false);
            setHalfCarryFlag(true);
            setCarryFlag(false);
        }

        void sub_set_flags(byte a, byte b)
        {
            if (b > a)
            {
                setCarryFlag(true);
            }
            else
            {
                setCarryFlag(false);
            }

            if (((a & 0xf) - (b & 0xf) & 0x10) > 0)
            {
                setHalfCarryFlag(true);
            }
            else
            {
                setHalfCarryFlag(false);
            }

            if (a - b == 0)
            {
                setZeroFlag(true);
            }
            else
            {
                setZeroFlag(false);
            }
            setNegativeFlag(true);
        }

        void dec_set_flags(byte a)
        {
            if ((((a & 0xf) - 1) & 0x10) > 0)
            {
                setHalfCarryFlag(true);
            }
            else
            {
                setHalfCarryFlag(false);
            }

            if (a == 0x1)
            {
                setZeroFlag(true);
            }
            else
            {
                setZeroFlag(false);
            }
            setNegativeFlag(true);
        }
        void inc_set_flags(byte a)
        {
            if ((((a & 0xf) + 1) & 0x10) > 0)
            {
                setHalfCarryFlag(true);
            }
            else
            {
                setHalfCarryFlag(false);
            }
            if (a == 0xff)
            {
                setZeroFlag(true);
            }
            else
            {
                setZeroFlag(false);
            }
            setNegativeFlag(false);
        }

        void or_set_flags(byte a, byte b)
        {
            if ((a | b) == 0)
            {
                setZeroFlag(true);
            }
            else
            {
                setZeroFlag(false);
            }
            setCarryFlag(false);
            setHalfCarryFlag(false);
            setNegativeFlag(false);
        }

        void xor_set_flags(byte a, byte b)
        {
            if ((a ^ b) == 0)
            {
                setZeroFlag(true);
            }
            else
            {
                setZeroFlag(false);
            }
            setCarryFlag(false);
            setHalfCarryFlag(false);
            setNegativeFlag(false);
        }

        // CPU OPERATIONS
        void adc_a_r8(byte r8)
        {
            AF.Upper += (byte)(r8 + getCarryFlagByte());
            add_set_flags(AF.Upper, (byte)(r8 + getCarryFlagByte()));
        }

        void adc_a_hl()
        { 
            AF.Upper += (byte) (_mem.Read(HL.Value) + getCarryFlagByte());
            add_set_flags(AF.Upper, (byte)(_mem.Read(HL.Value) + getCarryFlagByte()));

        }
        void adc_a_n8(byte n8)
        {
            AF.Upper += (byte)(n8 + getCarryFlagByte());
            add_set_flags(AF.Upper, (byte)(n8 + getCarryFlagByte()));
        }
        void add_a_r8(byte r8)
        {
            AF.Upper += r8;
            add_set_flags(AF.Upper, r8);
        }
        void add_a_hl()
        {
            AF.Upper += _mem.Read(HL.Value);
            add_set_flags(AF.Upper, _mem.Read(HL.Value));
        }
        void add_a_n8(byte n8)
        {
            AF.Upper += n8;
            add_set_flags(AF.Upper, n8);
        }
        void and_a_r8(byte r8)
        {
            AF.Upper &= r8;
            and_set_flags(AF.Upper, r8);
        }
        void and_a_hl()
        {
            AF.Upper &= _mem.Read(HL.Value);
            and_set_flags(AF.Upper, _mem.Read(HL.Value));
        }
        void and_a_n8(byte n8)
        {
            AF.Upper &= n8;
            and_set_flags(AF.Upper, n8);
        }
        void cp_a_r8(byte r8)
        {
            sub_set_flags(AF.Upper, r8);
        }
        void cp_a_hl()
        {
            sub_set_flags(AF.Upper, _mem.Read(HL.Value));
        }
        void cp_a_n8(byte n8)
        {
            sub_set_flags(AF.Upper, n8);
        }
        void dec_r8(ref byte r8)
        {
            r8--;
            dec_set_flags(r8);
        }
        void dec_hl()
        {
            _mem.Write(HL.Value, (byte)(_mem.Read(HL.Value) - 1));
            dec_set_flags(_mem.Read(HL.Value));
        }
        void inc_r8(ref byte r8)
        {
            r8++;
            inc_set_flags(r8);
        }
        void inc_hl()
        {
            _mem.Write(HL.Value, (byte)(_mem.Read(HL.Value) + 1));
            inc_set_flags(_mem.Read(HL.Value));
        }
        void or_a_r8(byte r8)
        {
            AF.Upper |= r8;
            or_set_flags(AF.Upper, r8);
        }
        void or_a_hl()
        {
            AF.Upper |= _mem.Read(HL.Value);
            or_set_flags(AF.Upper, _mem.Read(HL.Upper));
        }
        void or_a_n8(byte n8)
        {
            AF.Upper |= n8;
            or_set_flags(AF.Upper, n8);
        }
        void sbc_a_r8(byte r8)
        {
            AF.Upper -= r8;
            AF.Upper -= getCarryFlagByte();
            sub_set_flags(AF.Upper, (byte)(r8 + getCarryFlagByte()));
        }
        void sbc_a_hl()
        {
            AF.Upper -= _mem.Read(HL.Value);
            AF.Upper -= getCarryFlagByte();
            sub_set_flags(AF.Upper, (byte)(_mem.Read(HL.Value) + getCarryFlagByte()));
        }
        void sbc_a_n8(byte n8)
        {
            AF.Upper -= n8;
            AF.Upper -= getCarryFlagByte();
            sub_set_flags(AF.Upper, (byte)(n8 + getCarryFlagByte()));
        }
        void sub_a_r8(byte r8)
        {
            AF.Upper -= r8;
            sub_set_flags(AF.Upper, r8);
        }
        void sub_a_hl()
        {
            AF.Upper -= _mem.Read(HL.Value);
            sub_set_flags(AF.Upper, _mem.Read(HL.Value));
        }
        void sub_a_n8(byte n8)
        {
            AF.Upper -= n8;
            sub_set_flags(AF.Upper, n8);
        }
        void xor_a_r8(byte r8)
        {
            AF.Upper ^= r8;
            xor_set_flags(AF.Upper, r8);
        }
        void xor_a_hl()
        {
            AF.Upper ^= _mem.Read(HL.Value);
            xor_set_flags(AF.Upper ,_mem.Read(HL.Value));
        }

        void xor_a_n8(byte n8)
        {
            AF.Upper ^= n8;
            xor_set_flags(AF.Upper, n8);
        }

        void add_hl_r16(ushort r16)
        {
            
        }



        //------------------------------- LOAD FUNCTIONS -----------------------------//
        //Loads register to register
        void load_r8_r8(ref byte r, byte s)
        {
            r = s;
        }
        //Loads immediate to register
        void load_r8_n8(ref byte r8, byte n8)
        {
            r8 = n8;
        }
        //Loads 16 bit immediate to registers 
        void load_r16_n16(GBRegister r, ushort n16)
        {
            r.Value = n16;
        }
        //Load 16 bit register value into memory
        void load_mem_r8(ushort addr, byte r8)
        {
            _mem.Write(addr, r8);
        }
        //Load
        void load_r8_mem(ref byte r8, ushort addr)
        {
            r8 = _mem.Read(addr);
        }
        void load_mem_n8(ushort addr, byte n8)
        {
            _mem.Write(addr, n8);
        }

        byte get_immediate_8()
        {
            return _mem.Read((ushort)(PC.Value + 1));
        }
        ushort get_immediate_16()
        {
            //Assume little endian
            ushort imm = (ushort) ((_mem.Read((ushort)(PC.Value + 2)) << 8) | _mem.Read((ushort)(PC.Value + 1)));
            return imm;
        }


        void setZeroFlag(bool val)
        {
            if (val)
            {
                AF.Lower |= (1 << 7);
            }
            else
            {
                AF.Lower &= unchecked((byte)(~(1 << 7)));
            }
        }
        void setNegativeFlag(bool val)
        {
            if (val)
            {
                AF.Lower |= (1 << 6);
                
            }
            else
            {
                AF.Lower &= unchecked((byte)(~(1 << 6)));
            }
        }
        void setHalfCarryFlag(bool val)
        {
            if (val)
            {
                AF.Lower |= (1 << 5);
            }
            else
            {
                AF.Lower &= unchecked((byte)(~(1 << 5)));
            }
        }
        void setCarryFlag(bool val)
        {
            if (val)
            {
                AF.Lower |= (1 << 4);
            }
            else
            {
                AF.Lower &= unchecked((byte)(~(1 << 4)));
            }
        }
        bool getZeroFlag()
        {
            return (AF.Lower & (1 << 7)) > 0;
        }
        bool getCarryFlag()
        {
            return (AF.Lower & (1 << 4)) > 0;
        }
        bool getHalfCarryFlag()
        {
            return (AF.Lower & (1 << 5)) > 0;
        }
        bool getNegativeFlag()
        {
            return (AF.Lower & (1 << 6)) > 0;
        }

        byte getZeroFlagByte()
        {
            return (byte)((AF.Lower & (1 << 7)) >> 7);
        }
        byte getCarryFlagByte()
        {
            return (byte)((AF.Lower & (1 << 4)) >> 4);
        }
        byte getHalfCarryFlagByte()
        {
            return (byte)((AF.Lower & (1 << 5)) >> 5);
        }
        byte getNegativeFlagByte()
        {
            return (byte)((AF.Lower & (1 << 6)) >> 6);
        }
    }
    class GBRegister
    {
        public byte Upper { get; set; }
        public byte Lower { get; set; }
        public ushort Value
        {
            get
            {
                return (ushort) ((Upper << 8) | Lower);
            }
            set
            {
                Upper = (byte)(value >> 8);
                Lower = (byte)(value & 0xff);
            }
        }
    }
}
