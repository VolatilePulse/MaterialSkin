using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MaterialSkin.Controls
{
    public class MaterialScrim : Form
    {
        public new double Opacity
        {
            get => base.Opacity;
            set
            {
                if (value == base.Opacity) return;

                Visible = value != 0;
                base.Opacity = value;
            }
        }

        public MaterialScrim(Form owner)
        {
            InitializeComponent();

            Owner = owner ?? throw new ArgumentNullException(nameof(owner));
            Size = Owner.Size;
            Location = Owner.Location;

            Owner.Resize += (sender, e) => OnResize(e);
            Owner.Move += (sender, e) => OnMove(e);
            Owner.FormClosed += (sender, e) => OnFormClosed(e);
            Owner.Disposed += (sender, e) => Dispose();
            Owner.VisibleChanged += (sender, e) => OnVisibleChanged(e);
        }

        private void InitializeComponent()
        {
            BackColor = Color.Black;
            Opacity = 0;
            MinimizeBox = false;
            MaximizeBox = false;
            ShowIcon = false;
            ControlBox = false;
            Text = string.Empty;
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
        }

        public new void Show() => Opacity = 0.55;

        public new void Hide() => Opacity = 0;

        protected override void OnResize(EventArgs e)
        {
            if (Owner == null) return;
            Size = Owner.Size;
        }

        protected override void OnMove(EventArgs e)
        {
            if (Owner == null) return;
            Location = Owner.Location;
        }
    }

    public static class ScrimExtensions
    {
        public static void ShowScrim(this Form form) => GetScrim(form).Show();

        public static void HideScrim(this Form form) => GetScrim(form).Hide();

        private static MaterialScrim GetScrim(Form form)
        {
            var scrims = form.OwnedForms.Where(f => f is MaterialScrim);
            MaterialScrim scrim;

            if (!scrims.Any())
                scrim = new MaterialScrim(form);
            else
                scrim = scrims.First() as MaterialScrim;

            return scrim;
        }
    }

    public class DrawerScrim : MaterialScrim
    {
        public DrawerScrim(MaterialForm owner) : base(owner)
        {
            var rect = owner.RectangleToScreen(owner.UserArea);

            Size = rect.Size;
            Location = rect.Location;
        }

        protected override void OnResize(EventArgs e)
        {
            if (Owner == null) return;
            Size = (Owner as MaterialForm).UserArea.Size;
        }

        protected override void OnMove(EventArgs e)
        {
            if (Owner == null) return;
            Location = Owner.RectangleToScreen((Owner as MaterialForm).UserArea).Location;
        }
    }
}
