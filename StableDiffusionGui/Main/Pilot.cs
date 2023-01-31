using StableDiffusionGui.Data;
using StableDiffusionGui.Io;
using StableDiffusionGui.MiscUtils;
using StableDiffusionGui.Ui;
using System;
using System.Collections;
using System.Collections.Generic;
using static StableDiffusionGui.Main.Enums.StableDiffusion;

namespace StableDiffusionGui.Main
{
    internal class Pilot
    {
        static List<float> PilotIterationX = new List<float>();
        static List<float> PilotIterationY = new List<float>();

        enum PType
        {
            None,
            Seed,
            Prompt,
        };

        static PType XType;
        static PType YType;

        static public void Sync()
        {
            XType = (PType)Program.MainForm.cbXPilot.SelectedIndex;
            YType = (PType)Program.MainForm.cbYPilot.SelectedIndex;

            PilotIterationX.Clear();
            PilotIterationY.Clear();

            ReadFromType(XType, true);
            ReadFromType(YType, false);
        }

        static void ParseString(string Text, ref List<float> Data)
        {
            var TestA = Text.Split('-');

            if (TestA.Length > 1)
            {
                for (float i = TestA[0].Replace('.', ',').GetFloat(); i <= TestA[1].Replace('.', ',').GetFloat(); i++)
                {
                    Data.Add(i);
                }
            }
            else
            {
                var TestB = Text.Split(',');
                foreach (string i in TestB)
                {
                    Data.Add(i.GetFloat());
                }
            }
        }

        static void ReadFromType(PType Type, bool IsX)
        {
            switch (Type)
            {
                case PType.None: break;
                case PType.Seed:
                case PType.Prompt:
                    {
                        if (IsX)
                        {
                            ParseString(Program.MainForm.tbXPilot.Text, ref PilotIterationX);
                            break;
                        }
                        else
                        {
                            ParseString(Program.MainForm.tbYPilot.Text, ref PilotIterationY);
                            break;
                        }
                    }
            }
        }

        public static int GetTaskCount()
        {
            if (XType == PType.None)
                return 0;

            if (YType != PType.None)
            {
                return PilotIterationX.Count * PilotIterationY.Count;
            }
            else
            {
                return PilotIterationX.Count;
            }
        }

        static int SeedStep = 0;
        public static int Seed()
        {
            if (XType != PType.Seed && YType != PType.Seed)
                return (int)Program.MainForm.upDownSeed.Value;

            if (YType == PType.None)
            {
                int Step = (int)PilotIterationX[SeedStep];
                SeedStep++;

                return Step;
            }
            else
            {
                int Step = (int)PilotIterationX[SeedStep / PilotIterationX.Count];
                SeedStep++;

                return Step;
            }

            return (int)Program.MainForm.upDownSeed.Value;
        }

        public static void ClearTask()
        {
            SeedStep = 0;
        }
    }
}