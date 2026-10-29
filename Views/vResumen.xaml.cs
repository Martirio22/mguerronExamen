using System.Text;

namespace mguerronExamen.Views;

public partial class vResumen : ContentPage
{
    private readonly string usuarioConectado;

    public vResumen(
        string usuario,
        string nombre,
        string apellido,
        int edad,
        DateTime fechaInicio,
        string plan,
        bool casillero,
        decimal montoInicial,
        decimal pagoMensual,
        decimal pagoTotal)
    {
        InitializeComponent();

        usuarioConectado = usuario;

        lblUsuarioConectado.Text = "Usuario conectado: " + usuario;
        lblNombre.Text = nombre;
        lblApellido.Text = apellido;
        lblEdad.Text = edad.ToString();
        lblFechaInicio.Text = fechaInicio.ToString("dd/MM/yyyy");
        lblPlan.Text = plan;
        lblCasillero.Text = casillero ? "Sí" : "No";
        lblMontoInicial.Text = montoInicial.ToString("F2");
        lblPagoMensual.Text = pagoMensual.ToString("F2");
        lblPagoTotal.Text = pagoTotal.ToString("F2");
    }

    private async void btnNuevoRegistro_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new vRegistro(usuarioConectado));
    }

    private async void btnCerrarSesion_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }

    private async void btnExportar_Clicked(object sender, EventArgs e)
    {
        try
        {
            string contenido =
                "RESUMEN DEL SOCIO\n" +
                "Usuario conectado: " + usuarioConectado + "\n" +
                "Nombre: " + lblNombre.Text + "\n" +
                "Apellido: " + lblApellido.Text + "\n" +
                "Edad: " + lblEdad.Text + "\n" +
                "Fecha de inicio: " + lblFechaInicio.Text + "\n" +
                "Plan: " + lblPlan.Text + "\n" +
                "Casillero: " + lblCasillero.Text + "\n" +
                "Monto Inicial: $" + lblMontoInicial.Text + "\n" +
                "Pago mensual: $" + lblPagoMensual.Text + "\n" +
                "Pago Total: $" + lblPagoTotal.Text;

            string nombreArchivo =
                "ResumenSocio_" +
                DateTime.Now.ToString("yyyyMMdd_HHmmss_fff") +
                ".txt";

            string ruta = Path.Combine(
                FileSystem.AppDataDirectory,
                nombreArchivo
            );

            await File.WriteAllTextAsync(
                ruta,
                contenido,
                Encoding.UTF8
            );

            await DisplayAlert(
                "Archivo exportado", "Nombre: " + nombreArchivo + "\n" + "Ruta: " + ruta, "Aceptar"
            );
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Error", "No se pudo exportar el archivo: " + ex.Message, "Aceptar"
            );
        }
    }
}