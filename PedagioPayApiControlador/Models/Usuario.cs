﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace PedagioPayApiControlador.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string Nome { get; set; }

    public string Email { get; set; }

    public string CpfCnpj { get; set; }

    public string Celular { get; set; }

    public string Senha { get; set; }

    public bool? BlValidado { get; set; }

    public string CodigoValidacao { get; set; }

    public string Token { get; set; }

    public string IdFacebook { get; set; }

    public string IdGoogle { get; set; }

    public string IdApple { get; set; }

    public bool? CdStatus { get; set; }

    public DateTime? DhTimestamp { get; set; }
}