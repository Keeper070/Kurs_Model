using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kurs_Model_11
{
    public partial class Form1 : Form
    {
        // Делимое A.
        private ushort _a;
        // Делитель B.
        private ushort _b;
        // УА.
        private readonly ControlMachine _controlMachine;
        // Микропрограмма.
        private readonly Microprogram _microprogram;

        public Form1()
        {
            InitializeComponent();
            _controlMachine = new ControlMachine(this);
            _microprogram = new Microprogram(this);

            dataGridViewA.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dataGridViewB.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dataGridViewC5.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dataGridViewCount.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            dataGridViewA.Font = new Font("Times new Roman", 8);
            dataGridViewB.Font = new Font("Times new Roman", 8);
            dataGridViewC5.Font = new Font("Times new Roman", 8);
            dataGridViewCount.Font = new Font("Times new Roman", 8);
            const int widthColumn = 25;


            for (var i = 16 - 1; i >= 0; i--)
            {
                var index = dataGridViewA.Columns.Add("column_" + i, i.ToString());
                dataGridViewB.Columns.Add("column_" + i, i.ToString());
                dataGridViewA.Columns[index].Width = widthColumn;
                dataGridViewB.Columns[index].Width = widthColumn;

            }

            dataGridViewA.Height = 45;
            dataGridViewB.Height = 45;


            for (var i = 32 - 1; i >= 0; i--)
            {
                var index = dataGridViewAM.Columns.Add("column_" + i, i.ToString());
                dataGridViewBM.Columns.Add("column_" + i, i.ToString());
                dataGridViewC5.Columns.Add("column_" + i, i.ToString());
                dataGridViewAM.Columns[index].Width = widthColumn;
                dataGridViewBM.Columns[index].Width = widthColumn;
                dataGridViewC5.Columns[index].Width = widthColumn;
            }

            dataGridViewAM.Height = 45;
            dataGridViewBM.Height = 45;
            dataGridViewC5.Height = 45;

            for (var i = 4 - 1; i >= 0; i--)
            {
                var index = dataGridViewCount.Columns.Add("column_" + i, i.ToString());
                dataGridViewCount.Columns[index].Width = widthColumn;

            }
            dataGridViewCount.Height = 45;

            dataGridViewA.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            dataGridViewB.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            dataGridViewAM.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            dataGridViewBM.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            dataGridViewC5.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            dataGridViewCount.Rows.Add(0, 0, 0, 0);
        }

        #region Отображение данных 
        // Отображение данных в DataGritView 
        public void UpdateInfoRegister(uint am, uint bm, uint c, byte count)
        {
            // Отображение буфферного делимого -AM.
            var result = Convert.ToString(am, 2).PadLeft(32, '0');
            for (int i = 32 - 1, a = 31; i >= 0; i--, a--)
                dataGridViewAM.Rows[0].Cells[i].Value = result[a];

            // Отображение буфферного делителя - BM.
            result = Convert.ToString(bm, 2).PadLeft(32, '0');
            for (int i = 32 - 1, b = 31; i >= 0; i--, b--)
                dataGridViewBM.Rows[0].Cells[i].Value = result[b];

            // Отображение счетчика.
            result = Convert.ToString(count, 2).PadLeft(4, '0');
            for (var i = 4 - 1; i >= 0; i--)
                dataGridViewCount.Rows[0].Cells[i].Value = result[i];

            // Отображение частного - С.
            result = Convert.ToString(c, 2).PadLeft(32, '0');
            for (int i = 32 - 1, q = 31; i >= 0; i--, q--)
                dataGridViewC5.Rows[0].Cells[i].Value = result[q];
        }
        // Обновление состояния автомата.
        public void UpdateStateMemory(bool[] state)
        {
            radioButtonA0_0.Checked = state[0];
            radioButtonA1_1.Checked = state[1];
            radioButtonA2_2.Checked = state[2];
            radioButtonA3_3.Checked = state[3];
            radioButtonA4_4.Checked = state[4];
            radioButtonA5_5.Checked = state[5];
            radioButtonA6_6.Checked = state[6];
            radioButtonA7_7.Checked = state[7];
            radioButtonA8_8.Checked = state[8];
            radioButtonA9_9.Checked = state[9];


            for (var i = 0; i < state.Length; i++)
                checkedListBoxA.SetItemChecked(i, state[i]);
        }

        // Номер метки.
        public void UpdateA(ushort a)
        {
            radioButtonA0_0.Checked = a == 0;
            radioButtonA1_1.Checked = a == 1;
            radioButtonA2_2.Checked = a == 2;
            radioButtonA3_3.Checked = a == 3;
            radioButtonA4_4.Checked = a == 4;
            radioButtonA5_5.Checked = a == 5;
            radioButtonA6_6.Checked = a == 6;
            radioButtonA7_7.Checked = a == 7;
            radioButtonA8_8.Checked = a == 8;
            radioButtonA9_9.Checked = a == 9;
        }
        // Отображение Термы, KCY. KCD, ПЛУ
        public void UpdateTYDX(bool[] t, bool[] y, bool[] d, bool[] x)
        {
            for (var i = 0; i < t.Length; i++)
                checkedListBoxT.SetItemChecked(i, t[i]);
            for (var i = 0; i < y.Length; i++)
                checkedListBoxY.SetItemChecked(i, y[i]);
            for (var i = 0; i < d.Length; i++)
                checkedListBoxD.SetItemChecked(i, d[i]);
            for (var i = 0; i < x.Length; i++)
                checkedListBoxX.SetItemChecked(i, x[i]);
        }
        // Текущее состояние.
        public void UpdateInfoState(bool[] dt)
        {
            for (var i = 0; i < dt.Length; i++)
                checkedListBoxDt.SetItemChecked(i, dt[i]);
        }
        #endregion

        #region Обработчик нажатия на таблицу
        // Обработчик нажатия на таблицу А.
        private void dataGridViewA_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;

            var value = (int)dataGridViewA.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            value = value == 0 ? 1 : 0;
            dataGridViewA.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = value;
            var cells = dataGridViewA.Rows[e.RowIndex].Cells;
            var strB = new StringBuilder();
            for (var i = 0; i < cells.Count; i++)
                strB.Append(cells[i].Value);

            var denial = false;
            ushort a = 0;
            if ((int)cells[0].Value == 1)
            {
                a = (ushort)Convert.ToInt16(strB.ToString(), 2);
                strB.Replace("1", "0", 0, 1);
                denial = true;
            }

            _a = (ushort)Convert.ToInt16(strB.ToString(), 2);
            _a = denial ? a : _a;
        }
        // Обработчик нажатия на таблицу В.
        private void dataGridViewB_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;

            var value = (int)dataGridViewB.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            value = value == 0 ? 1 : 0;
            dataGridViewB.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = value;
            var cells = dataGridViewB.Rows[e.RowIndex].Cells;
            var strB = new StringBuilder();
            for (var i = 0; i < cells.Count; i++)
                strB.Append(cells[i].Value);

            var denial = false;
            ushort b = 0;
            if ((int)cells[0].Value == 1)
            {
                b = (ushort)Convert.ToInt16(strB.ToString(), 2);
                strB.Replace("1", "0", 0, 1);
                denial = true;
            }

            _b = (ushort)Convert.ToInt16(strB.ToString(), 2);
            _b = denial ? b : _b;
        }

        #endregion

        #region Кнопки
        // Кнопка старт для автоматического режима работы.
        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (radioButtonMicroprogram.Checked)
            {
                if (radioButtonAuto.Checked)
                {
                    _microprogram.Data(_a, _b);
                    _microprogram.Go();
                }
            }
            if (radioButtonYAOA.Checked)
            {
                if (radioButtonAuto.Checked)
                {
                    _controlMachine.Data(_a, _b);
                    _controlMachine.Go();
                }
            }
        }
        // Кнопка такт для потактового режима работы.
        private void buttonTact_Click(object sender, EventArgs e)
        {
            if (radioButtonMicroprogram.Checked)
            {
                if (radioButtonTact.Checked)
                {
                    _microprogram.Data(_a, _b);
                    _microprogram.Tact();
                }
            }
            if (radioButtonYAOA.Checked)
            {
                if (radioButtonTact.Checked)
                {
                    _controlMachine.Data(_a, _b);
                    _controlMachine.Tact();
                }
            }
        }

        // Кнопка сброса для очистки всех данных.
        private void buttonReset_Click(object sender, EventArgs e)
        {
            _microprogram.Reset();
            _controlMachine.Reset();
            ResetA();
            ResetB();
            ResetAM();
            ResetBM();
            ResetC();
            ResetCount();
        }
        // Кнопка сброса данных с таблицы А.
        private void buttonResetA_Click(object sender, EventArgs e)
        {
            ResetA();
        }
        // Метод для сброса А.
        private void ResetA()
        {
            for (var i = 0; i < dataGridViewA.Rows[0].Cells.Count; i++)
            {
                dataGridViewA.Rows[0].Cells[i].Value = 0;
            }

            _a = 0;
        }
        // Кнопка сброса данных с таблицы B.
        private void buttonResetB_Click(object sender, EventArgs e)
        {
            ResetB();
        }
        // Метод для сброса В.
        private void ResetB()
        {
            for (var i = 0; i < dataGridViewB.Rows[0].Cells.Count; i++)
            {
                dataGridViewB.Rows[0].Cells[i].Value = 0;
            }
            _b = 0;
        }
        // Кнопка сброса счетчика.
        private void ResetCount()
        {
            for (var i = 0; i < dataGridViewCount.Rows[0].Cells.Count; i++)
            {
                dataGridViewCount.Rows[0].Cells[i].Value = 0;
            }
        }
        // Метод для сброса данных с таблицы AM
        private void ResetAM()
        {
            for (var i = 0; i < dataGridViewAM.Rows[0].Cells.Count; i++)
            {
                dataGridViewAM.Rows[0].Cells[i].Value = 0;
            }
        }
        // Метод для сброса данных с таблицы BM
        private void ResetBM()
        {
            for (var i = 0; i < dataGridViewBM.Rows[0].Cells.Count; i++)
            {
                dataGridViewBM.Rows[0].Cells[i].Value = 0;
            }
        }
        // Метод для сброса данных с таблицы C
        private void ResetC()
        {
            for (var i = 0; i < dataGridViewC5.Rows[0].Cells.Count; i++)
            {
                dataGridViewC5.Rows[0].Cells[i].Value = 0;
            }
        }
        #endregion
    }
}