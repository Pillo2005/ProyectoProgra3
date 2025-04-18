Imports System.Data.SqlClient

Public Class FrmVentas
    Dim conBD As Conexion = New Conexion()
    Private Sub FrmVentas_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargarClientes()
        CargarProductos()
        txtEmpleado.Text = ModuloGlobal.NombreUsuario

        dtpFecha.Value = DateTime.Now

        With dgvDetalleVenta
            .Columns.Clear()
            .Columns.Add("IdProducto", "IdProducto")
            .Columns.Add("Nombre", "Nombre")
            .Columns.Add("Precio", "Precio")
            .Columns.Add("Cantidad", "Cantidad")
            .Columns.Add("PrecioUnitario", "PrecioUnitario")
        End With
    End Sub

    Private Sub CargarClientes()
        Dim consulta As String = "SELECT IdCliente, Nombre FROM Clientes"
        Dim adaptador As New SqlDataAdapter(consulta, conBD.Conectar())
        Dim tabla As New DataTable()
        adaptador.Fill(tabla)
        cboCliente.DataSource = tabla
        cboCliente.DisplayMember = "Nombre"
        cboCliente.ValueMember = "IdCliente"
        conBD.Cerrar()
    End Sub


    Private Sub CargarProductos()
        Dim consulta As String = "SELECT IdProducto, Nombre, Precio, Stock FROM Productos WHERE Stock > 0"
        Dim adaptador As New SqlDataAdapter(consulta, conBD.Conectar())
        Dim tabla As New DataTable()
        adaptador.Fill(tabla)
        dgvProductos.DataSource = tabla
        conBD.Cerrar()
    End Sub

    Private Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click

        If dgvProductos.SelectedRows.Count = 0 Then
            MessageBox.Show("Seleccione un producto de la lista.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If Not IsNumeric(txtCantidad.Text) OrElse CInt(txtCantidad.Text) <= 0 Then
            MessageBox.Show("Ingrese una cantidad válida.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim filaSeleccionada As DataGridViewRow = dgvProductos.CurrentRow
        Dim idProducto As Integer = Convert.ToInt32(filaSeleccionada.Cells("IdProducto").Value)
        Dim nombre As String = filaSeleccionada.Cells("Nombre").Value.ToString()
        Dim precio As Decimal = Convert.ToDecimal(filaSeleccionada.Cells("Precio").Value)
        Dim stock As Integer = Convert.ToInt32(filaSeleccionada.Cells("Stock").Value)
        Dim cantidad As Integer = Convert.ToInt32(txtCantidad.Text)

        If cantidad > stock Then
            MessageBox.Show("No hay suficiente stock disponible.", "Stock insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' Verificar si ya está en el carrito
        For Each fila As DataGridViewRow In dgvDetalleVenta.Rows
            If Convert.ToInt32(fila.Cells("IdProducto").Value) = idProducto Then
                MessageBox.Show("Este producto ya fue agregado al carrito.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
        Next

        Dim preciounitario As Decimal = cantidad * precio

        dgvDetalleVenta.Rows.Add(idProducto, nombre, precio, cantidad, preciounitario)

        CalcularTotal()
        txtCantidad.Clear()
    End Sub

    Private Sub CalcularTotal()
        Dim total As Decimal = 0

        For Each fila As DataGridViewRow In dgvDetalleVenta.Rows
            total += Convert.ToDecimal(fila.Cells("PrecioUnitario").Value)
        Next

         txtTotal.Text = total.ToString("F2")
    End Sub

    Private Sub btnGuardarVenta_Click(sender As Object, e As EventArgs) Handles btnGuardarVenta.Click

        If dgvDetalleVenta.Rows.Count = 0 Then
            MessageBox.Show("Debe agregar al menos un producto a la venta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Dim conexion As SqlConnection = conBD.Conectar()

        Try
            ' Insertar en tabla Ventas
            Dim cmdVenta As New SqlCommand("INSERT INTO Ventas (Fecha, IdCliente, IdEmpleado, Total) OUTPUT INSERTED.IdVenta VALUES (@Fecha, @IdCliente, @IdEmpleado, @Total)", conexion)
            cmdVenta.Parameters.AddWithValue("@Fecha", dtpFecha.Value)
            cmdVenta.Parameters.AddWithValue("@IdCliente", Convert.ToInt32(cboCliente.SelectedValue))
            cmdVenta.Parameters.AddWithValue("@IdEmpleado", ModuloGlobal.idEmpleado)
            cmdVenta.Parameters.AddWithValue("@Total", Convert.ToDecimal(txtTotal.Text))

            Dim idVentaGenerado As Integer = Convert.ToInt32(cmdVenta.ExecuteScalar())

            ' Insertar cada producto en DetalleVenta (individualmente)
            For Each fila As DataGridViewRow In dgvDetalleVenta.Rows
                Try
                    Dim idProducto As Integer = Convert.ToInt32(fila.Cells("IdProducto").Value)

                    ' Insertar en DetalleVenta
                    Dim cmdDetalle As New SqlCommand("INSERT INTO DetalleVenta (IdVenta, IdProducto, Cantidad, PrecioUnitario) VALUES (@IdVenta, @IdProducto, @Cantidad, @PrecioUnitario)", conexion)
                    cmdDetalle.Parameters.AddWithValue("@IdVenta", idVentaGenerado)
                    cmdDetalle.Parameters.AddWithValue("@IdProducto", idProducto)
                    cmdDetalle.Parameters.AddWithValue("@Cantidad", Convert.ToInt32(fila.Cells("Cantidad").Value))
                    cmdDetalle.Parameters.AddWithValue("@PrecioUnitario", Convert.ToDecimal(fila.Cells("PrecioUnitario").Value))
                    cmdDetalle.ExecuteNonQuery()

                    ' Actualizar stock del producto
                    Dim cmdStock As New SqlCommand("UPDATE Productos SET Stock = Stock - @Cantidad WHERE IdProducto = @IdProducto", conexion)
                    cmdStock.Parameters.AddWithValue("@Cantidad", Convert.ToInt32(fila.Cells("Cantidad").Value))
                    cmdStock.Parameters.AddWithValue("@IdProducto", idProducto)
                    cmdStock.ExecuteNonQuery()

                Catch exDetalle As Exception
                    MessageBox.Show("Error en producto con ID " & fila.Cells("IdProducto").Value & ": " & exDetalle.Message, "Error parcial", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End Try
            Next

            MessageBox.Show("Venta registrada (parcial o completa).", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ' LimpiarFormulario() (si tienes uno)
            CargarProductos()

        Catch ex As Exception
            MessageBox.Show("Error general al guardar la venta: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            conexion.Close()
        End Try

    End Sub



End Class