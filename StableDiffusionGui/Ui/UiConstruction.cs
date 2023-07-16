using StableDiffusionGui.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static StableDiffusionGui.Main.Constants;
using static System.Net.Mime.MediaTypeNames;

namespace StableDiffusionGui.Ui
{
    internal class UiConstruction
    {
        public enum PanelMovePos
        {
            First,
            Last,
            Above,
            Below
        }

        public static void SetPanelPosition(Panel targetPanel, PanelMovePos position, Panel referencePanel = null)
        {
            // Get the parent control
            Control parent = targetPanel.Parent;
            int referenceIndex = -1;

            if (position == PanelMovePos.Above || position == PanelMovePos.Below)
            {
                if (referencePanel == null)
                    return;

                referenceIndex = parent.Controls.GetChildIndex(referencePanel);
            }

            switch (position)
            {
                case PanelMovePos.Last:
                    parent.Controls.SetChildIndex(targetPanel, 0);
                    break;

                case PanelMovePos.First:
                    parent.Controls.SetChildIndex(targetPanel, parent.Controls.Count - 1);
                    break;

                case PanelMovePos.Below:
                    parent.Controls.SetChildIndex(targetPanel, referenceIndex == 0 ? 0 : referenceIndex);
                    break;

                case PanelMovePos.Above:
                    parent.Controls.SetChildIndex(targetPanel, referenceIndex + 1);
                    break;

                default:
                    throw new ArgumentException("Invalid panel target position specified.");
            }
        }


        public static Control CreateMainWindowOption(string name = "newControl", string text = "New Option")
        {
            Control parent = Program.MainForm.panelSettings;
            Control refPanel = parent.Controls.OfType<Panel>().Last();

            // Background/Parent
            Panel p = new Panel() { Name = GetUniqueControlName(name), Dock = DockStyle.Top, Size = refPanel.Size, BackColor = refPanel.BackColor };

            // Text
            Label label = new Label() { Name = GetUniqueControlName($"{name}Label"), Text = text };

            label.AutoSize = true;
            label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label.ForeColor = System.Drawing.Color.White;
            label.Location = new System.Drawing.Point(0, 4);
            label.Margin = new Padding(8, 0, 3, 0);

            p.Controls.Add(label);

            parent.Controls.Add(p);
            SetPanelPosition(p, PanelMovePos.Below, Program.MainForm.panelIterations);
            Control collapseParent = GetCollapseBtn(p);
            Program.MainForm.CategoryPanels[collapseParent].Add(p); // Add to collapse list
            Console.WriteLine($"Added {p.Name} panel to {parent.Name} - Category: {collapseParent.Name}");
            return p;
        }

        private static HTAlt.WinForms.HTButton GetCollapseBtn(Panel p)
        {
            Control parent = p.Parent;
            int childIndex = parent.Controls.GetChildIndex(p);

            Console.WriteLine($"Cat panels: {string.Join(", ", Program.MainForm.CategoryPanels.Keys.Select(k => k.Name))}");

            for (int i = childIndex; i < parent.Controls.Count; i++)
            {
                Control panel = parent.Controls[i];
                var btns = panel.Controls.OfType<HTAlt.WinForms.HTButton>().ToList();

                if (btns.Any() && Program.MainForm.CategoryPanels.Keys.Contains(btns.First()))
                    return btns.First();
            }

            return null;
        }

        public static string GetUniqueControlName(string preferredName, string nameTemplate = "{0}", int maxRetries = 1000000)
        {
            var controlNames = Program.MainForm.GetControls().Select(c => c.Name).ToList();
            int counter = 2;

            while (controlNames.Contains(preferredName))
            {
                preferredName += string.Format(nameTemplate, counter);
                counter++;

                if (counter > (maxRetries + 2))
                    break;
            }

            return preferredName;
        }
    }
}
