using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;
using HockeyApp.Android;

namespace Kalculadora
{
    [Activity(Label = "Calculador", MainLauncher = true, Icon = "@drawable/icon")]//Theme = "@android:style/Theme.Material.Light",,,
    public class MainActivity : Activity
    {
        #region PROPIEDADES
        public double ResultadoFinal { get; set; }
        public string ValorIngresado { get; set; }
        public bool OperacionRealizado { get; set; }
        public bool SelecOperacion { get; set; }
        public bool PrimerOperacion { get; set; }
        public bool PuntoPress { get; set; }
        public bool IgualPress { get; set; }
        public byte TipoPrimerOperacion { get; set; }
        public string valorOperar { get; set; }
        public string cadenaDeValores { get; set; }
        #endregion

        #region NUMERADORES
        enum OpcionOperacion
        {
            Suma = 1,
            Resta = 2,
            Multiplicacion = 3,
            Divicion = 4
        }

        enum Numeros
        {
            Cero = 0,
            Uno = 1,
            Dos = 2,
            Tres = 3,
            Cuatro = 4,
            Cinco = 5,
            Seis = 6,
            Siete = 7,
            Ocho = 8,
            Nueve = 9
        }
        #endregion

        protected override void OnCreate(Bundle bundle)
        {
            //MobileCenter.Start("ce564513-3acc-4fd6-9812-c28ab942efa2",
            //        typeof(Analytics), typeof(Crashes));//MobileAzure(seguimiento de la app)
            //CrashManager.Register(this, "c3cd94c2a99249f88f41c001034e2920");//(seguimiento de en hockeyapp)

            //RequestWindowFeature(WindowFeatures.NoTitle);//descomentar para no visualizar el action bar
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ActionBar.Title = " ";

            Button btnCero = FindViewById<Button>(Resource.Id.btn0);
            Button btnUno = FindViewById<Button>(Resource.Id.btn1);
            Button btnDos = FindViewById<Button>(Resource.Id.btn2);
            Button btnTres = FindViewById<Button>(Resource.Id.btn3);
            Button btnCuatro = FindViewById<Button>(Resource.Id.btn4);
            Button btnCinco = FindViewById<Button>(Resource.Id.btn5);
            Button btnSeis = FindViewById<Button>(Resource.Id.btn6);
            Button btnSiete = FindViewById<Button>(Resource.Id.btn7);
            Button btnOcho = FindViewById<Button>(Resource.Id.btn8);
            Button btnNueve = FindViewById<Button>(Resource.Id.btn9);
            Button btnMas = FindViewById<Button>(Resource.Id.btnMas);
            Button btnMenos = FindViewById<Button>(Resource.Id.btnMen);
            Button btnMulti = FindViewById<Button>(Resource.Id.btnMul);
            Button btnDivicion = FindViewById<Button>(Resource.Id.btnDiv);
            Button btnPunto = FindViewById<Button>(Resource.Id.btnPun);
            Button btnIgual = FindViewById<Button>(Resource.Id.btnIgual);
            Button btnLimpiar = FindViewById<Button>(Resource.Id.btnCle);
            EditText txtPantalla = FindViewById<EditText>(Resource.Id.txtValores);

            btnCero.Click += delegate { ConcatenarValores(txtPantalla, ((byte)Numeros.Cero).ToString()); };
            btnUno.Click += delegate { ConcatenarValores(txtPantalla, ((byte)Numeros.Uno).ToString()); };
            btnDos.Click += delegate { ConcatenarValores(txtPantalla, ((byte)Numeros.Dos).ToString()); };
            btnTres.Click += delegate { ConcatenarValores(txtPantalla, ((byte)Numeros.Tres).ToString()); };
            btnCuatro.Click += delegate { ConcatenarValores(txtPantalla, ((byte)Numeros.Cuatro).ToString()); };
            btnCinco.Click += delegate { ConcatenarValores(txtPantalla, ((byte)Numeros.Cinco).ToString()); };
            btnSeis.Click += delegate { ConcatenarValores(txtPantalla, ((byte)Numeros.Seis).ToString()); };
            btnSiete.Click += delegate { ConcatenarValores(txtPantalla, ((byte)Numeros.Siete).ToString()); };
            btnOcho.Click += delegate { ConcatenarValores(txtPantalla, ((byte)Numeros.Ocho).ToString()); };
            btnNueve.Click += delegate { ConcatenarValores(txtPantalla, ((byte)Numeros.Nueve).ToString()); };

            btnPunto.Click += delegate
            {
                if (PuntoPress == false)
                {
                    ConcatenarValores(txtPantalla, ".");
                    PuntoPress = true;
                }
            };

            btnMas.Click += delegate
            {
                if (valorOperar != string.Empty && txtPantalla.Text != string.Empty && txtPantalla.Text != "." && SelecOperacion == false)
                {
                    if (PrimerOperacion == false && TipoPrimerOperacion == (byte)Numeros.Cero)
                        TipoPrimerOperacion = (byte)OpcionOperacion.Suma;

                    //Operaciones(double.Parse(txtPantalla.Text), (byte)OpcionOperacion.Suma);
                    Operaciones(double.Parse(valorOperar), (byte)OpcionOperacion.Suma);
                    if (IgualPress)
                    {
                        txtPantalla.Text = cadenaDeValores = valorOperar + "+";
                        IgualPress = false;
                    }
                    else
                        txtPantalla.Text = cadenaDeValores = cadenaDeValores + "+";
                    OperacionRealizado = true;
                    SelecOperacion = true;
                    PuntoPress = false;
                }
            };

            btnMenos.Click += delegate
            {
                if (valorOperar != string.Empty && txtPantalla.Text != string.Empty && txtPantalla.Text != "." && SelecOperacion == false)
                {
                    if (PrimerOperacion == false && TipoPrimerOperacion == (byte)Numeros.Cero)
                        TipoPrimerOperacion = (byte)OpcionOperacion.Resta;

                    //Operaciones(double.Parse(txtPantalla.Text), (byte)OpcionOperacion.Resta);
                    Operaciones(double.Parse(valorOperar), (byte)OpcionOperacion.Resta);
                    if (IgualPress)
                    {
                        txtPantalla.Text = cadenaDeValores = valorOperar + "-";
                        IgualPress = false;
                    }
                    else
                        txtPantalla.Text = cadenaDeValores = cadenaDeValores + "-";
                    OperacionRealizado = true;
                    SelecOperacion = true;
                    PuntoPress = false;
                }
            };

            btnMulti.Click += delegate
            {
                if (valorOperar != string.Empty && txtPantalla.Text != string.Empty && txtPantalla.Text != "." && SelecOperacion == false)
                {
                    if (PrimerOperacion == false && TipoPrimerOperacion == (byte)Numeros.Cero)
                        TipoPrimerOperacion = (byte)OpcionOperacion.Multiplicacion;

                    //Operaciones(double.Parse(txtPantalla.Text), (byte)OpcionOperacion.Multiplicacion);
                    Operaciones(double.Parse(valorOperar), (byte)OpcionOperacion.Multiplicacion);
                    if (IgualPress)
                    {
                        txtPantalla.Text = cadenaDeValores = valorOperar + "x";
                        IgualPress = false;
                    }
                    else
                        txtPantalla.Text = cadenaDeValores = cadenaDeValores + "x";
                    OperacionRealizado = true;
                    SelecOperacion = true;
                    PuntoPress = false;
                }
            };

            btnDivicion.Click += delegate
            {
                if (valorOperar != string.Empty && txtPantalla.Text != string.Empty && txtPantalla.Text != "." && SelecOperacion == false)
                {
                    if (PrimerOperacion == false && TipoPrimerOperacion == (byte)Numeros.Cero)
                        TipoPrimerOperacion = (byte)OpcionOperacion.Divicion;

                    //Operaciones(double.Parse(txtPantalla.Text), (byte)OpcionOperacion.Divicion);
                    Operaciones(double.Parse(valorOperar), (byte)OpcionOperacion.Divicion);
                    if (IgualPress)
                    {
                        txtPantalla.Text = cadenaDeValores = valorOperar + "÷";
                        IgualPress = false;
                    }
                    else
                        txtPantalla.Text = cadenaDeValores = cadenaDeValores + "÷";
                    OperacionRealizado = true;
                    SelecOperacion = true;
                    PuntoPress = false;
                }
            };

            btnIgual.Click += delegate
            {
                if (valorOperar != string.Empty && txtPantalla.Text != string.Empty && txtPantalla.Text != ".")
                {
                    if (OperacionRealizado)
                    {
                        txtPantalla.Text = ResultadoFinal.ToString();
                        valorOperar = ResultadoFinal.ToString();
                    }
                    else if ((byte)TipoPrimerOperacion != (byte)Numeros.Cero)
                    {
                        //Operaciones(double.Parse(txtPantalla.Text), TipoPrimerOperacion);
                        Operaciones(double.Parse(valorOperar), TipoPrimerOperacion);
                        cadenaDeValores = ResultadoFinal.ToString();
                        txtPantalla.Text = ResultadoFinal.ToString();
                        valorOperar = ResultadoFinal.ToString();
                    }
                    //cadenaDeValores = string.Empty;
                    ResultadoFinal = (byte)Numeros.Cero;
                    ValorIngresado = string.Empty;
                    SelecOperacion = false;
                    TipoPrimerOperacion = (byte)Numeros.Cero;
                    PuntoPress = false;
                    IgualPress = true;
                }
            };

            btnLimpiar.Click += delegate
            {
                ResultadoFinal = (byte)Numeros.Cero;
                ValorIngresado = string.Empty;
                OperacionRealizado = false;
                valorOperar = string.Empty;
                txtPantalla.Text = string.Empty;
                cadenaDeValores = string.Empty;
                SelecOperacion = false;
                PrimerOperacion = false;
                TipoPrimerOperacion = (byte)Numeros.Cero;
                PuntoPress = false;
            };

            toolbar.MenuItemClick += (sender, e) =>
            {
                if (e.Item.ItemId == Resource.Id.menu_preferences)
                {
                    StartActivity(typeof(CreditosActivity));
                }
            };
        }

        #region METODOS

        public override bool OnCreateOptionsMenu(Android.Views.IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.top_menus, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        private void ConcatenarValores(EditText txtPantalla, string numero)
        {
            if (OperacionRealizado)
            {
                valorOperar = string.Empty;
                ValorIngresado = string.Empty;
            }

            if (IgualPress)
            {
                cadenaDeValores = numero;
                IgualPress = false;
            }
            else
                cadenaDeValores = cadenaDeValores + "" + numero;

            ValorIngresado = ValorIngresado + "" + numero;
            valorOperar = ValorIngresado;
            txtPantalla.Text = cadenaDeValores;
            OperacionRealizado = false;
            SelecOperacion = false;
        }

        private void Operaciones(double resultado, byte operacion)
        {
            if (ResultadoFinal != (byte)Numeros.Cero)
            {
                switch (TipoPrimerOperacion)
                {
                    case (byte)OpcionOperacion.Suma:
                        ResultadoFinal += resultado;
                        break;
                    case (byte)OpcionOperacion.Resta:
                        ResultadoFinal -= resultado;
                        break;
                    case (byte)OpcionOperacion.Multiplicacion:
                        ResultadoFinal *= resultado;
                        break;
                    case (byte)OpcionOperacion.Divicion:
                        ResultadoFinal /= resultado;
                        break;
                }
                TipoPrimerOperacion = operacion;
            }
            else
                ResultadoFinal = resultado;
        }
        #endregion
    }
}