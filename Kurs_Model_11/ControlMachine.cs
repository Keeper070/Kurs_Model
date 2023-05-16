namespace Kurs_Model_11
{
    public class ControlMachine
    {
        private readonly Form1 _form1;
        // Состояние автомата.
        private bool[] _aStates;
        // сигналы из KCY.
        public bool[] _ySignal { get; set; }
        // Сигналы из KCD.
        private bool[] _d;
        // Терма.
        private bool[] _t;
        // Предыдущее состояние.
        private int _preexisting;
        // Операционнный автомат
        private OperatingMachine _operatingMachine;
        public ControlMachine(Form1 form1)
        {
            _form1 = form1;
            _aStates = new bool[10];
            _ySignal = new bool[20];
            _t = new bool[19];
            _d = new bool[5];
            _preexisting = 0;
        }
        
        // Внесение данных полученое с главной формы.
        public void Data(ushort a, ushort b)
        {
            _operatingMachine = new OperatingMachine(this,a,b);
            _operatingMachine._x[0] = true;
        }
       
        // Автоматический режим работы.
         public void Go()    
        {
            while (!_operatingMachine.ready )
            {
              Tact();  
            }
        }

        // Такт.
        public void Tact()
        {
            if(_operatingMachine.ready )
            {
                _form1.UpdateA(0);
                return;
            }
            _form1.UpdateInfoState(_d);

            StateMemory( Decoder(_d));
            Terma(_t);
            CSY();
            СSD(_d);
            _operatingMachine.Tact();

            // Отображение
            _form1.UpdateInfoRegister(_operatingMachine.am,_operatingMachine.bm,_operatingMachine.c,_operatingMachine.count);
            _form1.UpdateStateMemory(_aStates);
            _form1.UpdateTYDX(_t, _ySignal, _d, _operatingMachine._x);
        }

        // Память состояний.
        private void StateMemory(int _current)
        {
            _aStates[_preexisting] = false;
            _aStates[_current] = true;
            _preexisting = _current;
        }
        
        // Дешифратор.
        private int Decoder(bool[] _d)
        {
            var _current = 0;
            if (_d[0])
            {
                _current = 1;
                _d[0] = false;
            }
            if (_d[1])
            {
                _current += 2;
                _d[1] = false;
            }
            if (_d[2])
            {
                _current += 4;
                _d[2] = false;
            }
            if (_d[3])
            {
                _current += 8;
                _d[3] = false;
            } 
            if (_d[4])
            {
                _current += 16;
                _d[4] = false;
            }

            return _current;
        }

        // Комбинационная схема D.
        private void СSD(bool[] _d)
        {
            _d[0] = _d[1] = _d[2] = _d[3] = false;
            _d[0] = _t[2] || _t[4] || _t[6] || _t[7] || _t[10] || _t[12] || _t[13] || _t[14] || _t[15] || _t[16] ||
                    _t[17] || _t[18];
            _d[1] = _t[3] || _t[4] || _t[8] || _t[9] || _t[10];
            _d[2]= _t[5] || _t[6] || _t[7] || _t[8] || _t[9] || _t[10];
            _d[3] = _t[11] || _t[12] || _t[13] || _t[14] || _t[15] || _t[16] || _t[17] || _t[18];
        }

        // Терма.
        private void Terma(bool[] _t)
        {
            _t[0] = _t[1] = _t[2] = _t[3] = _t[4] = _t[5] = _t[6] = _t[7] = _t[8] =
                _t[9] = _t[10] = _t[11] = _t[12] = _t[13] = _t[14] = _t[15] = _t[16] = _t[17] = _t[18]  = false;
            _t[0] = _aStates[0] && !_operatingMachine._x[0];
            _t[1] = _aStates[9];
            _t[2] = _aStates[0] && _operatingMachine._x[0];
            _t[3] = _aStates[1] && !_operatingMachine._x[1] && !_operatingMachine._x[2];
            _t[4] = _aStates[2] && _operatingMachine._x[3];
            _t[5] = _aStates[3];
            _t[6] = _aStates[4];
            _t[7] = _aStates[7] && !_operatingMachine._x[5];
            _t[8] = _aStates[5] && !_operatingMachine._x[4];
            _t[9] = _aStates[5] && _operatingMachine._x[4];
            _t[10] = _aStates[6];
            _t[11] = _aStates[7] && _operatingMachine._x[5] && _operatingMachine._x[6];
            _t[12] = _aStates[8] && _operatingMachine._x[7];
            _t[13] = _aStates[8] && !_operatingMachine._x[7];
            _t[14] = _aStates[7] && _operatingMachine._x[5] && !_operatingMachine._x[6] && !_operatingMachine._x[7];
            _t[15]=_aStates[7] && _operatingMachine._x[5] && !_operatingMachine._x[6] && _operatingMachine._x[7];
            _t[16] = _aStates[1] && _operatingMachine._x[1];
            _t[17] = _aStates[1] && !_operatingMachine._x[1] && _operatingMachine._x[2];
            _t[18] = _aStates[2] && !_operatingMachine._x[3];
        }
        
        // Комбинационная схема Y.
        private void CSY()
        {
            for (int i = 0; i < _ySignal.Length; i++)
            {
                _ySignal[i] = false;
            }
            //if(_t[0])
            if (_t[1]) _ySignal[17] = true;
            if (_t[2])
            {
                _ySignal[0] = true;
                _ySignal[1] = true;
                _ySignal[2] = true;
            }
            if (_t[3] || _t[6] || _t[7])
            {
                _ySignal[3] = true;
            }
            if (_t[4]) _ySignal[4] = true;
            if (_t[5])
            {
                _ySignal[5] = true;
                _ySignal[6] = true;
                _ySignal[7] = true;
                _ySignal[8] = true;
            }
            if (_t[8])
            {
                _ySignal[9] = true;
                _ySignal[10] = true;
            }
            if (_t[9])
            {
                _ySignal[11] = true;
                _ySignal[12] = true;
            }
            if (_t[10])
            {
                _ySignal[13] = true;
                _ySignal[14] = true;
            }
            if (_t[11]) _ySignal[15] = true;
            if (_t[12] || _t[15]) _ySignal[16] = true;
            // else if(_t[14]) 
            //else if(_t[15])
            if (_t[17])
            {
                _ySignal[19] = true;
            }
            if (_t[16] || _t[18]) _ySignal[19] = true;
        }
        // Сброс.
        public void Reset()
        {
            _aStates = new bool[10];
            _ySignal = new bool[20];
            _t = new bool[19];
            _d = new bool[5];
            _preexisting = 0;
            _operatingMachine = new OperatingMachine(this, 0, 0);
        }
    }
}