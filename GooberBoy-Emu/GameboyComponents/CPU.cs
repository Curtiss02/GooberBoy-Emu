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

            setCarryFlag(byte.MaxValue - a < b);
            setHalfCarryFlag((a & 0xf + b & 0xf & 0x10) > 0);
            setZeroFlag(a + b == 0);
            setNegativeFlag(false);
        }

        void add_16_set_flags(ushort a, ushort b)
        {

            setCarryFlag(ushort.MaxValue - a < b);
            setCarryFlag(false);
            setHalfCarryFlag((a >> 8 & 0x0f + b >> 8 & 0x0f & 0x10) > 0);
            setNegativeFlag(false);
            
        }
        void and_set_flags(byte a, byte b)
        {

            setZeroFlag((a & b) == 0);
            setNegativeFlag(false);
            setHalfCarryFlag(true);
            setCarryFlag(false);
        }

        void sub_set_flags(byte a, byte b)
        {
            setCarryFlag(b > a);
            setHalfCarryFlag(((a & 0xf) - (b & 0xf) & 0x10) > 0);
            setZeroFlag(a - b == 0);
            setNegativeFlag(true);
        }

        void dec_set_flags(byte a)
        {

            setHalfCarryFlag((((a & 0xf) - 1) & 0x10) > 0);
            setZeroFlag(a == 0x1);
            setNegativeFlag(true);
        }
        void inc_set_flags(byte a)
        {
            setHalfCarryFlag((((a & 0xf) + 1) & 0x10) > 0);
            setZeroFlag(a == 0xff);
            setNegativeFlag(false);
        }

        void or_set_flags(byte a, byte b)
        {

            setZeroFlag((a | b) == 0);

            setCarryFlag(false);
            setHalfCarryFlag(false);
            setNegativeFlag(false);
        }

        void xor_set_flags(byte a, byte b)
        {

            setZeroFlag((a ^ b) == 0);
            setCarryFlag(false);
            setHalfCarryFlag(false);
            setNegativeFlag(false);
        }

        // CPU OPERATIONS
        void adc_a_r8(byte r8)
        {
            AF.Upper += (byte)(r8 + getCarryFlagBit());
            add_set_flags(AF.Upper, (byte)(r8 + getCarryFlagBit()));
        }

        void adc_a_hl()
        { 
            AF.Upper += (byte) (_mem.Read(HL.Value) + getCarryFlagBit());
            add_set_flags(AF.Upper, (byte)(_mem.Read(HL.Value) + getCarryFlagBit()));

        }
        void adc_a_n8(byte n8)
        {
            AF.Upper += (byte)(n8 + getCarryFlagBit());
            add_set_flags(AF.Upper, (byte)(n8 + getCarryFlagBit()));
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
            AF.Upper -= getCarryFlagBit();
            sub_set_flags(AF.Upper, (byte)(r8 + getCarryFlagBit()));
        }
        void sbc_a_hl()
        {
            AF.Upper -= _mem.Read(HL.Value);
            AF.Upper -= getCarryFlagBit();
            sub_set_flags(AF.Upper, (byte)(_mem.Read(HL.Value) + getCarryFlagBit()));
        }
        void sbc_a_n8(byte n8)
        {
            AF.Upper -= n8;
            AF.Upper -= getCarryFlagBit();
            sub_set_flags(AF.Upper, (byte)(n8 + getCarryFlagBit()));
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
            HL.Value += r16;
            add_16_set_flags(HL.Value, r16);
        }

        void dec_r16(ref ushort r16)
        {
            r16 -= 1;
        }
        void inc_r16(ref ushort r16)
        {
            r16 += 1;
        }
        //------------------------------- BIT OPERATIONS INSTRUCTIONS -----------------------------//
        void bit_u3_r8(byte u3, byte r8)
        {
            int result = r8 & (1 << u3);
            setZeroFlag(result == 0);
            setNegativeFlag(false);
            setHalfCarryFlag(true);
        }

        void bit_u3_hl(byte u3)
        {
            int result = _mem.Read(HL.Value) & (1 << u3);
            setZeroFlag(result == 0);
            setNegativeFlag(false);
            setHalfCarryFlag(true);
        }

        void res_u3_r8(byte u3, ref byte r8)
        {
            r8 &= (byte) ~(1 << u3);
        }

        void res_u3_hl(byte u3)
        {
            _mem.Write(HL.Value, (byte) (_mem.Read(HL.Value) & ~(1 << u3)));
        }

        void set_u3_r8(byte u3, byte r8)
        {
            r8 |= (byte) (1 << u3);
        }

        void set_u3_hl(byte u3)
        {
            _mem.Write(HL.Value, (byte) (_mem.Read(HL.Value) | (1 << u3)));
        }

        void swap_r8(ref byte r8)
        {
            r8 = (byte) ((r8 << 4) | (r8 >> 4));
        }

        void swap_hl()
        {
            _mem.Write((HL.Value), (byte)((_mem.Read(HL.Value) >> 4) | (_mem.Read(HL.Value) << 4))); 
        }
        //------------------------------- BIT SHIFT INSTRUCTIONS -----------------------------//


        void rl_r8(ref byte r8)
        {
            byte carry = getCarryFlagBit();
            setCarryFlag(r8 >> 7 > 0);
            r8 = (byte) (r8 << 1 | carry);
            setZeroFlag(r8 == 0);
            setNegativeFlag(false);
            setHalfCarryFlag(false);
        }

        void rl_hl()
        {
            byte val = _mem.Read(HL.Value);
            byte carry = getCarryFlagBit();
            setCarryFlag(val >> 7 > 0);
            val = (byte) (val << 1 | carry); 
            _mem.Write(HL.Value, val);
            setZeroFlag(val == 0);
            setNegativeFlag(false);
            setHalfCarryFlag(false);
        }
        void rla()
        {
            byte carry = getCarryFlagBit();
            setCarryFlag(AF.Upper >> 7 > 0);
            AF.Upper = (byte) (AF.Upper << 1 | carry);
            setZeroFlag(false);
            setHalfCarryFlag(false);
            setNegativeFlag(false);
        }

        void rlc_r8(ref byte r8)
        {
            setCarryFlag(AF.Upper >> 7 > 0);
            r8 = (byte)(r8 << 1 | r8 >> 7);
            setZeroFlag(r8 == 0);
            setNegativeFlag(false);
            setHalfCarryFlag(false);
        }

        void rlc_hl()
        {
            byte val = _mem.Read(HL.Value);
            setCarryFlag(val >> 7 > 0);
            val = (byte) (val << 1 |  val >> 7); 
            _mem.Write(HL.Value, val);
            setZeroFlag(val == 0);
            setNegativeFlag(false);
            setHalfCarryFlag(false);
        }

        void rlca()
        {
            setCarryFlag(AF.Upper >> 7 > 0);
            AF.Upper = (byte)(AF.Upper << 1 | AF.Upper >> 7);
            setZeroFlag(false);
            setNegativeFlag(false);
            setHalfCarryFlag(false);
        }

        void rr_r8(ref byte r8)
        {
            byte carry = getCarryFlagBit();
            setCarryFlag((r8 & 1) > 0);
            r8 = (byte) (r8 >> 1 | (carry << 7));
            setZeroFlag(r8 == 0);
            setNegativeFlag(false);
            setHalfCarryFlag(false);
        }
        void rr_hl()
        {
            byte val = _mem.Read(HL.Value);
            byte carry = getCarryFlagBit();
            setCarryFlag((val & 1) > 0);
            val = (byte) (val >> 1 | (carry << 7));
            _mem.Write(HL.Value, val);
            setZeroFlag(val == 0);
            setNegativeFlag(false);
            setHalfCarryFlag(false);
        }

        void rra()
        {
            byte carry = getCarryFlagBit();
            setCarryFlag((AF.Upper & 1) > 0);
            AF.Upper = (byte) (AF.Upper >> 1 | (carry << 7));
            setZeroFlag(false);
            setNegativeFlag(false);
            setHalfCarryFlag(false);
        }

        void rrc_r8(ref byte r8)
        {
            setCarryFlag((r8 & 1) > 0);
            r8 = (byte) (r8 >> 1  | r8 << 7);
            setZeroFlag(r8 == 0);
            setNegativeFlag(false);
            setHalfCarryFlag(false);
        }

        void rrc_hl()
        {
            byte val = _mem.Read(HL.Value);
            setCarryFlag((val & 1) > 0);
            val = (byte) (val >> 1 | val << 7);
            _mem.Write(HL.Value, val);
            setZeroFlag(val == 0);
            setNegativeFlag(false);
            setHalfCarryFlag(false);
        }

        void rrc_a()
        {
            setCarryFlag((AF.Upper & 1) > 0);
            AF.Upper  = (byte) (AF.Upper  >> 1 | AF.Upper << 7);
            setZeroFlag(false);
            setNegativeFlag(false);
            setHalfCarryFlag(false); 
        }

        void sla_r8(ref byte r8)
        {
            setCarryFlag(r8 >> 7 > 0);
            r8 = (byte) (r8 << 1);
            setZeroFlag(r8 == 0);
            setHalfCarryFlag(false);
            setNegativeFlag(false);
            
        }

        void sla_hl()
        {
            byte val = _mem.Read(HL.Value);
            setCarryFlag(val >> 7 > 0);
            val = (byte) (val << 1); 
            _mem.Write(HL.Value, val);
            setZeroFlag(val == 0);
            setNegativeFlag(false);
            setHalfCarryFlag(false);
        }

        void sra_r8(ref byte r8)
        {
            setCarryFlag((r8 & 1) > 0);
            r8 = (byte) (r8 >> 1 | r8 << 7);
            setZeroFlag(r8 == 0);
            setHalfCarryFlag(false);
            setNegativeFlag(false);
        }
        
        void sra_hl()
        {
            byte val = _mem.Read(HL.Value);
            setCarryFlag((val & 1) > 0);
            val = (byte) (val >> 1 | val << 7);
            _mem.Write(HL.Value, val);
            setZeroFlag(val == 0);
            setHalfCarryFlag(false);
            setNegativeFlag(false);
        }

        void srl_r8(ref byte r8)
        {
            setCarryFlag((r8 & 1) > 0);
            r8 = (byte) (r8 >> 1);
            setZeroFlag(r8 == 0);
            setHalfCarryFlag(false);
            setNegativeFlag(false);
        }

        void srl_hl()
        {
            byte val = _mem.Read(HL.Value);
            setCarryFlag((val & 1) > 0);
            val = (byte) (val >> 1);
            _mem.Write(HL.Value, val);
            setZeroFlag(val == 0);
            setHalfCarryFlag(false);
            setNegativeFlag(false);
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
        void load_r16_n16(ushort r16, ushort n16)
        {
            r16 = n16;
        }
        //Load 16 bit register value into memory
        void load_hl_r8(byte r8)
        {
            _mem.Write(HL.Value, r8);
        }
        void load_hl_n8(byte n8)
        {
            _mem.Write(HL.Value, n8);
        }
        //Load
        void load_r8_hl(ref byte r8)
        {
            r8 = _mem.Read(HL.Value);
        }
        
        
        // OTHER HELPER FUNCS
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

        byte getZeroFlagBit()
        {
            return (byte)((AF.Lower & (1 << 7)) >> 7);
        }
        byte getCarryFlagBit()
        {
            return (byte)((AF.Lower & (1 << 4)) >> 4);
        }
        byte getHalfCarryFlagBit()
        {
            return (byte)((AF.Lower & (1 << 5)) >> 5);
        }
        byte getNegativeFlagBit()
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
