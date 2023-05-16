using System;

namespace Kurs_Model_11
{
    public class Microprogram
    {
        private Form1 _form1;
        // Делимое.
        private ushort _a;
        // Делитель.
        private ushort _b;
        // Буфферная переменная для делимого.
        private uint _am;
        // Буфферная переменная для делителя.
        private uint _bm;
        private uint _m;
        // Счетчик.
        private byte count;
        // Частное.
        private uint _c;
        // Массив микроопераций.
        private Action[] masAction;
        //Логическое услогие.
        private bool[] _x;
        // Переполнение
        private bool _overflow;
        // Переменная, говорящая о завершении работы автомата.
        private bool ready;
        // Состояние.
        private byte _condition;
        public Microprogram(Form1 form1)
        {
            _form1 = form1;
            _x = new bool[8];
            _x[0] = true;
            _condition = 0;
            // Массив микроопераций.
            masAction = new Action[]
            {
                // y1-y3.
                () => { _am = (uint)(_a & 0x7FFF); },
                () => { _bm = (uint)(_b & 0x7FFF); },
                () => { _overflow = false; },
                // y4.
                () =>
                {
                    var buff = _bm & 0x7FFF;
                    buff = (~buff) | 0xC0000000;
                    _am += buff + 0x01;
                },
                // y5.
                () => { _am += _bm  & 0x7FFF; },
                // y6-9.
                () => { _m = _am << 1; },
                () => { _am <<= 1; },
                () => { count = 0; },
                () => { _c = _c >> 16 << 16; },
                // y10-11.
                () => { _am <<= 1; },
                () => { _c = (_c << 1) + 1; },
                // y12-3.
                () => { _am = _m << 1; },
                () => { _c <<= 1; },
                // y14-15.
                () => { _m = _am; },
                () =>
                {
                    count = (byte)(count == 0
                        ? count = 15
                        : count - 1);
                },
                // y16.
                () =>
                {
                    var buffC = _c + 2;
                    buffC = buffC << 15 >> 15;
                    _c = _c >> 17 << 17;
                    _c += buffC;
                },
                // y17. 
                () => { _c |= 0x10000; },
                //y18
                () => { ready = true; },
                // y19.
                () => { _c = 0; },
                // y20.
                () => { _overflow = true; }
            };
        }
        // Внесение данных полученое с главной формы.
        public void Data(ushort a, ushort b)
        {
            _a = a;
            _b = b;
        }
        // Автоматический режим.
        public void Go()
        {
            while (!ready)
            {
                Tact();
            }
        }
        // Потактовый режим.
        public void Tact()
        {
            switch (_condition)
            {
                case 0:
                    if (_x[0])
                    {
                        masAction[0]();
                        masAction[1]();
                        masAction[2]();
                        _condition = 1;
                    }
                    break;
                case 1:
                    if (_x[1])
                    {
                        masAction[19]();
                        _condition = 9;
                    }
                    else
                    {
                        if (_x[2])
                        {
                            masAction[18]();
                            _condition = 9;
                        }
                        else
                        {
                            masAction[3]();
                            _condition = 2;
                        }
                    }
                    break;
                case 2:
                    if (_x[3])
                    {
                        masAction[4]();
                        _condition = 3;
                    }
                    else
                    {
                        masAction[19]();
                        _condition = 9;
                    }
                    break;
                case 3:
                    masAction[5]();
                    masAction[6]();
                    masAction[7]();
                    masAction[8]();
                    _condition = 4;
                    break;
                case 4:
                    masAction[3]();
                    _condition = 5;
                    break;
                case 5:
                    if (_x[4])
                    {
                        masAction[11]();
                        masAction[12]();
                    }
                    else
                    {
                        masAction[9]();
                        masAction[10]();
                    }
                    _condition = 6;
                    break;
                case 6:
                    masAction[13]();
                    masAction[14]();
                    _condition = 7;
                    break;
                case 7:
                    if (count != 0)
                    {
                        _condition = 4;
                    }
                    else
                    {
                        if (_x[6])
                        {
                            masAction[15]();
                            _condition = 8;
                        }
                        else
                        {
                            if (_x[7])
                            {
                                masAction[16]();
                            }
                            _condition = 9;
                        }
                    }
                    break;
                case 8:
                    if (_x[7])
                    {
                        masAction[16]();
                    }
                    _condition = 9;
                    break;
                case 9:
                    masAction[17]();
                    break;
            }
            _form1.UpdateInfoRegister(_am, _bm, _c, count);
            _form1.UpdateA(_condition);
            LogicalDevice(_x);
        }

        // Память логических условий.
        public void LogicalDevice(bool[] _x)
        {
            //_x[0] = _form1.isRun;
            _x[1] = (_bm & 0x7FFF) == 0;
            _x[2] = (_am & 0x7FFF) == 0;
            _x[3] = (_am & 0x10000) != 0;
            _x[4] = (_am & 0x10000) != 0;
            _x[5] = count == 0;
            _x[6] = (_c & 0x1) != 0;
            _x[7] = (_a ^ _b) == 1;
        }
        // Сброс.
        public void Reset()
        {
            ready = false;
            _a = 0;
            _b = 0;
            _x = new bool[8];
            _x[0] = false;
            _condition = 0;
        }
    }
}