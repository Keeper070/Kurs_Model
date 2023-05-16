using System;

namespace Kurs_Model_11
{
    public class OperatingMachine
    {
        private Form1 _form1;
        // Управляющий автомат
        private ControlMachine _controlMachine;
        // Делимое
        private ushort _a;
        // Делитель
        private ushort _b;
        // Буфферная переменная для делимого.
        public uint am;
        // Буфферная переменная для делителя.
        public uint bm;
        private uint _m;
        // Счетчик.
        public byte count;
        // Частное.
        public uint c;
        // Массив микроопераций
        private Action[] masAction;
        // Логические условия.
        public bool[] _x { get; set; }
        // Переполнение
        private bool _overflow;
        // Переменная, говорящая о завершении работы автомата.
        public bool ready { get; set; }

        public OperatingMachine(ControlMachine controlMachine,ushort a,ushort b)
        {
            _controlMachine = controlMachine;
            _a = a;
            _b = b;
            am = 0;
            bm = 0;
            _m = 0;
            count = 0;
            c = 0;
            _x = new bool[8];
            // Массив микроопераций.
            masAction = new Action[]
            {
                // y1-y3.
                () => { am = (uint)(_a & 0x7FFF); },
                () => { bm = (uint)(_b & 0x7FFF); },
                () => { _overflow = false; },
                // y4.
                () =>
                {
                    var buff = bm & 0x7FFF;
                    buff = (~buff) | 0xC0000000;
                    am += buff + 0x01;
                },
                // y5.
                () => { am += bm  & 0x7FFF; },
                // y6-9.
                () => { _m = am << 1; },
                () => { am <<= 1; },
                () => { count = 0; },
                () => { c = c >> 16 << 16; },
                // y10-11.
                () => { am <<= 1; },
                () => { c = (c << 1) + 1; },
                // y12-3.
                () => { am = (_m << 1); },
                () => { c <<= 1; },
                // y14-15.
                () => { _m = am; },
                () =>
                {
                    count = (byte)(count == 0
                        ? count = 15    
                        : count - 1);
                },
                // y16.
                () =>
                {
                    var buffC = c + 2;
                    buffC = buffC << 15 >> 15;
                    c = c >> 17 << 17;
                    c += buffC;
                },
                // y17. 
                () => { c |= 0x10000; },
                //y18
                () => { ready = true; },
                // y19.
                () => { c = 0; },
                // y20.
                () => { _overflow = true; }
            };
        }

        // Такт.
        public void Tact()
        {
            for (var i = 0; i < _controlMachine._ySignal.Length; i++)
            {
                if (_controlMachine._ySignal[i])
                {
                    masAction[i]();
                }
            }

            LogicalDevice(_x);
        }

        // Память логических условий.
        public void LogicalDevice(bool[] _x)
        {
            //_x[0] = _form1.isRun;
            _x[1] = (bm & 0x7FFF) == 0;
            _x[2] = (am & 0x7FFF) == 0;
            _x[3] = (am & 0x10000) != 0;
            _x[4] = (am & 0x10000) != 0;
            _x[5] = count == 0;
            _x[6] = (c & 0x1) != 0;
            _x[7] = (_a ^ _b) == 1;
        }

        public void Reset()
        {
            ready = false;
            am = 0;
            bm = 0;
            _m = 0;
            count = 0;
            c = 0;
            _x = new bool[8];
        }
    }
}