namespace CCore.Senary.Gameplay.Attacking
{
    public class BattleResult
    {
        private int throwResult;

        private int unitCount;

        public int ThrowResult { get { return throwResult; } }

        public int UnitCount { get { return unitCount; } }

        public int FinalResult { get { return throwResult * unitCount; } }

        public BattleResult(int throwResult, int unitCount)
        {
            this.throwResult = throwResult;

            this.unitCount = unitCount;
        }
    }
}