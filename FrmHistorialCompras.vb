Imports System.Data.SqlClient
Public Class lblBienvenida

    Dim conBD As Conexion = New Conexion()

    Public IdClienteSeleccionado As Integer
    Public NombreCliente As String
    Private Sub lblBienvenida_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblCliente.Text = "Historial de compras de: " & NombreCliente
        MostrarHistorial()
    End Sub

    Private Sub MostrarHistorial()
        Dim consulta As String = "SELECT V.IdVenta, V.Fecha, P.Nombre AS Producto, DV.Cantidad, DV.PrecioUnitario 
                                  FROM Ventas V 
                                  INNER JOIN DetalleVenta DV ON V.IdVenta = DV.IdVenta
                                  INNER JOIN Productos P ON DV.IdProducto = P.IdProducto
                                  WHERE V.IdCliente = @idCliente"

        Dim comando As New SqlCommand(consulta, conBD.Conectar())
        comando.Parameters.AddWithValue("@idCliente", IdClienteSeleccionado)

        Dim adaptador As New SqlDataAdapter(comando)
        Dim tabla As New DataTable()
        adaptador.Fill(tabla)
        dgvHistorial.DataSource = tabla

        conBD.Cerrar()
    End Sub

    Private Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        Me.Close()
    End Sub

End Class