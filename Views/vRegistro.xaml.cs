using System.Globalization;

namespace mguerronExamen.Views;

public partial class vRegistro : ContentPage
{
    private const decimal costoMembresia = 1200;
    private const decimal porcentajeRecargo = 0.02m;
    private const decimal costoCasillero = 10;
    private const int numeroCuotas = 5;

    private readonly string usuarioConectado;

    public vRegistro(string usuario)
    {
        InitializeComponent();

        usuarioConectado = usuario;
        lblUsuarioConectado.Text = "Usuario conectado: " + usuario;

        dtpFechaInicio.MinimumDate = DateTime.Today;
        dtpFechaInicio.Date = DateTime.Today;
    }

    private async Task<bool> ValidarDatos()
    {
        if (string.IsNullOrWhiteSpace(txtNombre.Text))
        {
            await DisplayAlert("Error", "Ingrese el nombre", "OK");
            return false;
        }

        if (string.IsNullOrWhiteSpace(txtApellido.Text))
        {
            await DisplayAlert("Error", "Ingrese el apellido", "OK");
            return false;
        }

        if (!int.TryParse(txtEdad.Text, out int edad) || edad < 16)
        {
            await DisplayAlert("Error",
                "La edad debe ser un número entero mayor o igual a 16", "OK");
            return false;
        }

        if (dtpFechaInicio.Date < DateTime.Today)
        {
            await DisplayAlert("Error", "La fecha de inicio no puede ser anterior a hoy", "OK");
            return false;
        }

        if (pckPlan.SelectedIndex == -1)
        {
            await DisplayAlert("Error", "Seleccione un plan", "OK");
            return false;
        }

        if (!ObtenerMontoInicial(out decimal montoInicial) ||
            montoInicial < costoMembresia * 0.10m ||
            montoInicial > costoMembresia * 0.50m)
        {
            await DisplayAlert("Error", "El monto inicial debe estar entre $120 y $600", "OK");
            return false;
        }

        return true;
    }

    private bool ObtenerMontoInicial(out decimal monto)
    {
        return decimal.TryParse(
            txtMontoInicial.Text,
            NumberStyles.Number,
            CultureInfo.CurrentCulture,
            out monto);
    }

    private decimal CalcularPagoMensual(decimal montoInicial)
    {
        decimal saldoRestante = costoMembresia - montoInicial;
        decimal cuotaBase = saldoRestante / numeroCuotas;
        decimal recargo = costoMembresia * porcentajeRecargo;
        decimal casillero = swCasillero.IsToggled ? costoCasillero : 0;

        return cuotaBase + recargo + casillero;
    }

    private async void btnCalcular_Clicked(object sender, EventArgs e)
    {
        txtPagoMensual.Text = "";

        if (!await ValidarDatos())
            return;

        ObtenerMontoInicial(out decimal montoInicial);

        decimal pagoMensual = CalcularPagoMensual(montoInicial);

        txtPagoMensual.Text = pagoMensual.ToString("F2");
    }

    private async void btnVerResumen_Clicked(object sender, EventArgs e)
    {
        if (!await ValidarDatos())
            return;

        ObtenerMontoInicial(out decimal montoInicial);

        decimal pagoMensual = CalcularPagoMensual(montoInicial);
        decimal saldoRestante = costoMembresia - montoInicial;
        decimal totalCuotas = pagoMensual * numeroCuotas;
        decimal totalPagar = montoInicial + totalCuotas;

        txtPagoMensual.Text = pagoMensual.ToString("F2");

        string resumen =
            "Usuario conectado: " + usuarioConectado + "\n" +
            "Nombre: " + txtNombre.Text!.Trim() + "\n" +
            "Apellido: " + txtApellido.Text!.Trim() + "\n" +
            "Edad: " + txtEdad.Text + "\n" +
            "Fecha de inicio: " + dtpFechaInicio.Date?.ToString("dd/MM/yyyy") + "\n" +
            "Plan: " + pckPlan.SelectedItem + "\n" +
            "Casillero: " + (swCasillero.IsToggled ? "Sí" : "No") + "\n" +
            "Costo membresía: $" + costoMembresia.ToString("F2") + "\n" +
            "Monto inicial: $" + montoInicial.ToString("F2") + "\n" +
            "Saldo restante: $" + saldoRestante.ToString("F2") + "\n" +
            "Número de cuotas: " + numeroCuotas + "\n" +
            "Pago mensual: $" + pagoMensual.ToString("F2") + "\n" +
            "Total a pagar: $" + totalPagar.ToString("F2");

        await DisplayAlert("Resumen del socio", resumen, "Aceptar");
    }
}