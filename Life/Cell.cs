using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace life
{
    public class Cell //ячейка
    {
        public bool IsAlive; //живой
        public readonly List<Cell> neighbors = new List<Cell>(); //соседи

        private bool IsAliveNext; //жив в следующем поколении

        public void DetermineNextLiveState() //определить следующее состояние
        {
            int liveNeighbors = neighbors.Where(x => x.IsAlive).Count(); //определяяется кол-во живых соседей

            if (IsAlive) //если он сейчас жив, нужно два или три соседа чтобы продолжал жить
                IsAliveNext = liveNeighbors == 2 || liveNeighbors == 3;
            else //если сейчас мертв, родится если рядом ровно три живые клетки
                IsAliveNext = liveNeighbors == 3;
        }

        public void Advance() //переходим как бы в следующее поколение
        {
            IsAlive = IsAliveNext;
        }
    }
}
