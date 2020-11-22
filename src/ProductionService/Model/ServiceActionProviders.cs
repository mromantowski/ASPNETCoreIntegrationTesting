using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductionService.Model
{
    public class Saw1ActionProvider : ServiceActionProviderBase
    {
        public override Machine Machine => Machine.Saw1;

        public override IList<string> Actions =>
            new[] 
            {
                "Opróżnij pojemnik na odpady",
                "Sprawdź stan ostrza"
            };
    }

    public class Saw1CalibrationActionProvider : ServiceActionProviderBase
    {
        private readonly DayOfWeek[] Days = new DayOfWeek[] { DayOfWeek.Tuesday, DayOfWeek.Thursday, DayOfWeek.Saturday };
        public override Machine Machine => Machine.Saw1;

        public override bool DateMatches(DateTime day) => Days.Contains(day.DayOfWeek);

        public override IList<string> Actions =>
            new[]
            {
                "Wykonaj kalibrację urządzenia",
            };
    }

    public class Saw2ActionProvider : ServiceActionProviderBase
    {
        public override Machine Machine => Machine.Saw2;

        public override IList<string> Actions =>
            new[]
            {
                "Sprawdź stan ostrza"
            };
    }

    public class Saw2CalibrationActionProvider : ServiceActionProviderBase
    {
        private readonly DayOfWeek[] Days = new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Thursday };
        public override Machine Machine => Machine.Saw2;

        public override bool DateMatches(DateTime day) => Days.Contains(day.DayOfWeek);

        public override IList<string> Actions =>
            new[]
            {
                "Wykonaj kalibrację urządzenia",
            };
    }

    public class Saw2StartMonthActionProvider : ServiceActionProviderBase
    {
        public override Machine Machine => Machine.Saw2;

        public override bool DateMatches(DateTime day) 
            => day.DayOfWeek == DayOfWeek.Monday && day.Day <= 7;

        public override IList<string> Actions =>
            new[]
            {
                 "Nasmaruj rolki"
            };
    }

    public class CofeeMakerActionProvider : ServiceActionProviderBase
    {
        public override Machine Machine => Machine.CoffeeMaker;

        public override IList<string> Actions =>
            new[]
            {
                "Umyj kubek"
            };
    }
}
