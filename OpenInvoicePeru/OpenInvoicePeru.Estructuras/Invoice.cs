﻿using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using OpenInvoicePeru.Comun;
using OpenInvoicePeru.Comun.Constantes;

namespace OpenInvoicePeru.Estructuras
{
    [Serializable]
    public class Invoice : IXmlSerializable, IEstructuraXml
    {
        public DateTime IssueDate { get; set; }
        public UblExtensions UblExtensions { get; set; }
        public SignatureCac Signature { get; set; }
        public AccountingSupplierParty AccountingSupplierParty { get; set; }
        public string InvoiceTypeCode { get; set; }
        public string Id { get; set; }
        public AccountingSupplierParty AccountingCustomerParty { get; set; }
        public List<InvoiceLine> InvoiceLines { get; set; }
        public List<InvoiceDocumentReference> DespatchDocumentReferences { get; set; }
        public string DocumentCurrencyCode { get; set; }
        public List<TaxTotal> TaxTotals { get; set; }
        public LegalMonetaryTotal LegalMonetaryTotal { get; set; }
        public BillingPayment PrepaidPayment { get; set; }
        public string UblVersionId { get; set; }
        public string CustomizationId { get; set; }
        public IFormatProvider Formato { get; set; }

        public Invoice()
        {
            AccountingSupplierParty = new AccountingSupplierParty();
            AccountingCustomerParty = new AccountingSupplierParty();
            DespatchDocumentReferences = new List<InvoiceDocumentReference>();
            UblExtensions = new UblExtensions();
            Signature = new SignatureCac();
            InvoiceLines = new List<InvoiceLine>();
            TaxTotals = new List<TaxTotal>();
            LegalMonetaryTotal = new LegalMonetaryTotal();
            UblVersionId = "2.0";
            CustomizationId = "1.0";
            Formato = new System.Globalization.CultureInfo("es-PE");
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("xmlns", EspacioNombres.xmlnsInvoice);
            writer.WriteAttributeString("xmlns:cac", EspacioNombres.cac);
            writer.WriteAttributeString("xmlns:cbc", EspacioNombres.cbc);
            writer.WriteAttributeString("xmlns:ccts", EspacioNombres.ccts);
            writer.WriteAttributeString("xmlns:ds", EspacioNombres.ds);
            writer.WriteAttributeString("xmlns:ext", EspacioNombres.ext);
            writer.WriteAttributeString("xmlns:qdt", EspacioNombres.qdt);
            writer.WriteAttributeString("xmlns:sac", EspacioNombres.sac);
            writer.WriteAttributeString("xmlns:udt", EspacioNombres.udt);
            writer.WriteAttributeString("xmlns:xsi", EspacioNombres.xsi);

            #region UBLExtensions
            writer.WriteStartElement("ext:UBLExtensions");

            #region UBLExtension
            var ext2 = UblExtensions.Extension2.ExtensionContent.AdditionalInformation;
            writer.WriteStartElement("ext:UBLExtension");

            #region ExtensionContent
            writer.WriteStartElement("ext:ExtensionContent");

            #region AdditionalInformation
            writer.WriteStartElement("sac:AdditionalInformation");
            {

                #region AdditionalMonetaryTotal

                {

                    foreach (var additionalMonetaryTotal in ext2.AdditionalMonetaryTotals)
                    {
                        writer.WriteStartElement("sac:AdditionalMonetaryTotal");
                        writer.WriteElementString("cbc:ID", additionalMonetaryTotal.Id);

                        #region PayableAmount

                        {
                            writer.WriteStartElement("cbc:PayableAmount");
                            {
                                writer.WriteAttributeString("currencyID", additionalMonetaryTotal.PayableAmount.CurrencyId);
                                writer.WriteValue(additionalMonetaryTotal.PayableAmount.Value.ToString(Formatos.FormatoNumerico, Formato));

                            }
                            writer.WriteEndElement();
                        }
                        if (additionalMonetaryTotal.Percent > 0)
                        {
                            writer.WriteElementString("cbc:Percent",
                                additionalMonetaryTotal.Percent.ToString(Formatos.FormatoNumerico));
                        }
                        #endregion

                        writer.WriteEndElement();
                    }
                }

                #endregion

                #region AdditionalProperty

                {
                    foreach (var additionalProperty in ext2.AdditionalProperties)
                    {
                        writer.WriteStartElement("sac:AdditionalProperty");
                        writer.WriteElementString("cbc:ID", additionalProperty.Id);

                        #region Value

                        writer.WriteElementString("cbc:Value", additionalProperty.Value);

                        #endregion

                        writer.WriteEndElement();
                    }
                }

                #endregion

                #region SUNATEmbededDespatchAdvice
                // Para el caso de Factura-Guia.
                if (!string.IsNullOrEmpty(ext2.SunatEmbededDespatchAdvice.DeliveryAddress.Id))
                {
                    writer.WriteStartElement("sac:SUNATEmbededDespatchAdvice");
                    {
                        #region DeliveryAddress
                        writer.WriteStartElement("cac:DeliveryAddress");
                        {
                            writer.WriteElementString("cbc:ID", ext2.SunatEmbededDespatchAdvice.DeliveryAddress.Id);
                            writer.WriteElementString("cbc:StreetName", ext2.SunatEmbededDespatchAdvice.DeliveryAddress.StreetName);
                            if (!string.IsNullOrEmpty(ext2.SunatEmbededDespatchAdvice.DeliveryAddress.CitySubdivisionName))
                                writer.WriteElementString("cbc:CitySubdivisionName", ext2.SunatEmbededDespatchAdvice.DeliveryAddress.CitySubdivisionName);
                            writer.WriteElementString("cbc:CityName", ext2.SunatEmbededDespatchAdvice.DeliveryAddress.CityName);
                            writer.WriteElementString("cbc:CountrySubentity", ext2.SunatEmbededDespatchAdvice.DeliveryAddress.CountrySubentity);
                            writer.WriteElementString("cbc:District", ext2.SunatEmbededDespatchAdvice.DeliveryAddress.District);
                            writer.WriteStartElement("cac:Country");
                            {
                                writer.WriteElementString("cbc:IdentificationCode", ext2.SunatEmbededDespatchAdvice.DeliveryAddress.Country.IdentificationCode);
                            }
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                        #endregion

                        #region OriginAddress
                        writer.WriteStartElement("cac:OriginAddress");
                        {
                            writer.WriteElementString("cbc:ID", ext2.SunatEmbededDespatchAdvice.OriginAddress.Id);
                            writer.WriteElementString("cbc:StreetName", ext2.SunatEmbededDespatchAdvice.OriginAddress.StreetName);
                            if (!string.IsNullOrEmpty(ext2.SunatEmbededDespatchAdvice.OriginAddress.CitySubdivisionName))
                                writer.WriteElementString("cbc:CitySubdivisionName", ext2.SunatEmbededDespatchAdvice.OriginAddress.CitySubdivisionName);
                            writer.WriteElementString("cbc:CityName", ext2.SunatEmbededDespatchAdvice.OriginAddress.CityName);
                            writer.WriteElementString("cbc:CountrySubentity", ext2.SunatEmbededDespatchAdvice.OriginAddress.CountrySubentity);
                            writer.WriteElementString("cbc:District", ext2.SunatEmbededDespatchAdvice.OriginAddress.District);
                            writer.WriteStartElement("cac:Country");
                            {
                                writer.WriteElementString("cbc:IdentificationCode", ext2.SunatEmbededDespatchAdvice.OriginAddress.Country.IdentificationCode);
                            }
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                        #endregion

                        #region SUNATCarrierParty
                        writer.WriteStartElement("sac:SUNATCarrierParty");
                        {
                            writer.WriteElementString("cbc:CustomerAssignedAccountID", ext2.SunatEmbededDespatchAdvice.SunatCarrierParty.CustomerAssignedAccountId);
                            writer.WriteElementString("cbc:AdditionalAccountID", ext2.SunatEmbededDespatchAdvice.SunatCarrierParty.AdditionalAccountId);
                            writer.WriteStartElement("cac:Party");
                            {
                                writer.WriteStartElement("cac:PartyLegalEntity");
                                {
                                    writer.WriteElementString("cbc:RegistrationName", ext2.SunatEmbededDespatchAdvice.SunatCarrierParty.Party.PartyLegalEntity.RegistrationName);
                                }
                                writer.WriteEndElement();
                            }
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                        #endregion

                        #region DriverParty
                        writer.WriteStartElement("sac:DriverParty");
                        {
                            writer.WriteStartElement("cac:Party");
                            {
                                writer.WriteStartElement("cac:PartyIdentification");
                                {
                                    writer.WriteElementString("cbc:ID", ext2.SunatEmbededDespatchAdvice.DriverParty.PartyIdentification.Id.Value);
                                }
                                writer.WriteEndElement();
                            }
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                        #endregion

                        #region SUNATRoadTransport
                        writer.WriteStartElement("sac:SUNATRoadTransport");
                        {
                            writer.WriteElementString("cbc:LicensePlateID", ext2.SunatEmbededDespatchAdvice.SunatRoadTransport.LicensePlateId);
                            writer.WriteElementString("cbc:TransportAuthorizationCode", ext2.SunatEmbededDespatchAdvice.SunatRoadTransport.TransportAuthorizationCode);
                            writer.WriteElementString("cbc:BrandName", ext2.SunatEmbededDespatchAdvice.SunatRoadTransport.BrandName);
                        }
                        writer.WriteEndElement();
                        #endregion

                        writer.WriteElementString("cbc:TransportModeCode", ext2.SunatEmbededDespatchAdvice.TransportModeCode);

                        #region GrossWeightMeasure
                        writer.WriteStartElement("cbc:GrossWeightMeasure");
                        {
                            writer.WriteAttributeString("unitCode", ext2.SunatEmbededDespatchAdvice.GrossWeightMeasure.UnitCode);
                            writer.WriteValue(ext2.SunatEmbededDespatchAdvice.GrossWeightMeasure.Value.ToString(Formatos.FormatoNumerico, Formato));
                        }
                        writer.WriteEndElement();
                        #endregion
                    }
                    writer.WriteEndElement();
                }
                #endregion

                #region SUNATCosts
                if (!string.IsNullOrEmpty(ext2.SunatCosts.RoadTransport.LicensePlateId))
                {
                    writer.WriteStartElement("sac:SUNATCosts");
                    {
                        writer.WriteStartElement("cac:RoadTransport");
                        {
                            writer.WriteElementString("cbc:LicensePlateID", ext2.SunatCosts.RoadTransport.LicensePlateId);
                        }
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                }
                #endregion

                #region SUNATTransaction
                if (!string.IsNullOrEmpty(ext2.SunatTransaction.Id) 
                    && string.IsNullOrEmpty(ext2.SunatCosts.RoadTransport.LicensePlateId))
                {
                    writer.WriteStartElement("sac:SUNATTransaction");
                    {
                        writer.WriteElementString("cbc:ID", ext2.SunatTransaction.Id);
                    }
                    writer.WriteEndElement();
                }
                #endregion

            }
            writer.WriteEndElement();
            #endregion

            writer.WriteEndElement();
            #endregion

            writer.WriteEndElement();
            #endregion

            #region UBLExtension

            writer.WriteStartElement("ext:UBLExtension");
            #region ExtensionContent
            writer.WriteStartElement("ext:ExtensionContent");

            // En esta zona va el certificado digital.

            writer.WriteEndElement();
            #endregion
            writer.WriteEndElement();
            #endregion

            writer.WriteEndElement();
            #endregion

            writer.WriteElementString("cbc:UBLVersionID", UblVersionId);
            writer.WriteElementString("cbc:CustomizationID", CustomizationId);
            writer.WriteElementString("cbc:ID", Id);
            writer.WriteElementString("cbc:IssueDate", IssueDate.ToString("yyyy-MM-dd"));
            writer.WriteElementString("cbc:InvoiceTypeCode", InvoiceTypeCode);
            writer.WriteElementString("cbc:DocumentCurrencyCode", DocumentCurrencyCode);

            #region DespatchDocumentReferences
            foreach (var reference in DespatchDocumentReferences)
            {
                writer.WriteStartElement("cac:DespatchDocumentReference");
                {
                    writer.WriteElementString("cbc:ID", reference.Id);
                    writer.WriteElementString("cbc:DocumentTypeCode", reference.DocumentTypeCode);
                }
                writer.WriteEndElement();
            }
            #endregion

            #region Signature
            writer.WriteStartElement("cac:Signature");
            writer.WriteElementString("cbc:ID", Signature.Id);

            #region SignatoryParty

            writer.WriteStartElement("cac:SignatoryParty");

            writer.WriteStartElement("cac:PartyIdentification");
            writer.WriteElementString("cbc:ID", Signature.SignatoryParty.PartyIdentification.Id.Value);
            writer.WriteEndElement();

            #region PartyName
            writer.WriteStartElement("cac:PartyName");

            //writer.WriteStartElement("cbc:Name");
            //writer.WriteCData(Signature.SignatoryParty.PartyName.Name);
            //writer.WriteEndElement();
            writer.WriteElementString("cbc:Name", Signature.SignatoryParty.PartyName.Name);

            writer.WriteEndElement();
            #endregion

            writer.WriteEndElement();
            #endregion

            #region DigitalSignatureAttachment
            writer.WriteStartElement("cac:DigitalSignatureAttachment");

            writer.WriteStartElement("cac:ExternalReference");
            writer.WriteElementString("cbc:URI", Signature.DigitalSignatureAttachment.ExternalReference.Uri.Trim());
            writer.WriteEndElement();

            writer.WriteEndElement();
            #endregion

            writer.WriteEndElement();
            #endregion

            #region AccountingSupplierParty
            writer.WriteStartElement("cac:AccountingSupplierParty");

            writer.WriteElementString("cbc:CustomerAssignedAccountID", AccountingSupplierParty.CustomerAssignedAccountId);
            writer.WriteElementString("cbc:AdditionalAccountID",
                AccountingSupplierParty.AdditionalAccountId);

            #region Party
            writer.WriteStartElement("cac:Party");

            #region PartyName
            writer.WriteStartElement("cac:PartyName");

            writer.WriteStartElement("cbc:Name");
            writer.WriteCData(AccountingSupplierParty.Party.PartyName.Name);
            writer.WriteEndElement();

            writer.WriteEndElement();
            #endregion

            #region PostalAddress
            writer.WriteStartElement("cac:PostalAddress");
            writer.WriteElementString("cbc:ID", AccountingSupplierParty.Party.PostalAddress.Id);
            writer.WriteElementString("cbc:StreetName", AccountingSupplierParty.Party.PostalAddress.StreetName);
            if (!string.IsNullOrEmpty(AccountingSupplierParty.Party.PostalAddress.CitySubdivisionName))
                writer.WriteElementString("cbc:CitySubdivisionName", AccountingSupplierParty.Party.PostalAddress.CitySubdivisionName);
            writer.WriteElementString("cbc:CityName", AccountingSupplierParty.Party.PostalAddress.CityName);
            writer.WriteElementString("cbc:CountrySubentity", AccountingSupplierParty.Party.PostalAddress.CountrySubentity);
            writer.WriteElementString("cbc:District", AccountingSupplierParty.Party.PostalAddress.District);

            #region Country
            writer.WriteStartElement("cac:Country");
            writer.WriteElementString("cbc:IdentificationCode",
                AccountingSupplierParty.Party.PostalAddress.Country.IdentificationCode);
            writer.WriteEndElement();
            #endregion

            writer.WriteEndElement();
            #endregion

            #region PartyLegalEntity
            writer.WriteStartElement("cac:PartyLegalEntity");

            writer.WriteStartElement("cbc:RegistrationName");
            writer.WriteCData(AccountingSupplierParty.Party.PartyLegalEntity.RegistrationName);
            writer.WriteEndElement();

            writer.WriteEndElement();
            #endregion

            writer.WriteEndElement();
            #endregion

            writer.WriteEndElement();
            #endregion

            #region AccountingCustomerParty
            writer.WriteStartElement("cac:AccountingCustomerParty");

            writer.WriteElementString("cbc:CustomerAssignedAccountID", AccountingCustomerParty.CustomerAssignedAccountId);
            writer.WriteElementString("cbc:AdditionalAccountID",
                AccountingCustomerParty.AdditionalAccountId);

            #region Party
            writer.WriteStartElement("cac:Party");

            #region cbc:PartyLegalEntity
            writer.WriteStartElement("cac:PartyLegalEntity");

            writer.WriteStartElement("cbc:RegistrationName");
            writer.WriteCData(AccountingCustomerParty.Party.PartyLegalEntity.RegistrationName);
            writer.WriteEndElement();

            writer.WriteEndElement();
            #endregion

            writer.WriteEndElement();
            #endregion

            writer.WriteEndElement();
            #endregion

            #region PrepaidPayment
            if (PrepaidPayment != null)
            {
                writer.WriteStartElement("cac:PrepaidPayment");
                {
                    writer.WriteStartElement("cbc:ID");
                    {
                        writer.WriteAttributeString("schemeID", PrepaidPayment.Id.SchemeId);
                        writer.WriteValue(PrepaidPayment.Id.Value);
                    }
                    writer.WriteEndElement();
                    writer.WriteStartElement("cbc:PaidAmount");
                    {
                        writer.WriteAttributeString("currencyID", PrepaidPayment.PaidAmount.CurrencyId);
                        writer.WriteValue(PrepaidPayment.PaidAmount.Value.ToString(Formatos.FormatoNumerico, Formato));
                    }
                    writer.WriteEndElement();
                    writer.WriteStartElement("cbc:InstructionID");
                    {
                        writer.WriteAttributeString("schemeID", "6");
                        writer.WriteValue(PrepaidPayment.InstructionId);
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
            #endregion

            #region TaxTotal
            foreach (var taxTotal in TaxTotals)
            {
                writer.WriteStartElement("cac:TaxTotal");

                writer.WriteStartElement("cbc:TaxAmount");
                writer.WriteAttributeString("currencyID", taxTotal.TaxAmount.CurrencyId);
                writer.WriteString(taxTotal.TaxAmount.Value.ToString(Formatos.FormatoNumerico, Formato));
                writer.WriteEndElement();

                #region TaxSubtotal
                {
                    writer.WriteStartElement("cac:TaxSubtotal");

                    writer.WriteStartElement("cbc:TaxAmount");
                    writer.WriteAttributeString("currencyID", taxTotal.TaxSubtotal.TaxAmount.CurrencyId);
                    writer.WriteString(taxTotal.TaxAmount.Value.ToString(Formatos.FormatoNumerico, Formato));
                    writer.WriteEndElement();

                    #region TaxCategory

                    {
                        writer.WriteStartElement("cac:TaxCategory");

                        #region TaxScheme
                        {
                            writer.WriteStartElement("cac:TaxScheme");

                            writer.WriteElementString("cbc:ID", taxTotal.TaxSubtotal.TaxCategory.TaxScheme.Id);
                            writer.WriteElementString("cbc:Name", taxTotal.TaxSubtotal.TaxCategory.TaxScheme.Name);
                            writer.WriteElementString("cbc:TaxTypeCode", taxTotal.TaxSubtotal.TaxCategory.TaxScheme.TaxTypeCode);

                            writer.WriteEndElement();
                        }
                        #endregion

                        writer.WriteEndElement();
                    }
                    #endregion

                    writer.WriteEndElement();
                }
                #endregion

                writer.WriteEndElement();
            }
            #endregion

            #region LegalMonetaryTotal
            writer.WriteStartElement("cac:LegalMonetaryTotal");
            {
                if (LegalMonetaryTotal.AllowanceTotalAmount.Value > 0)
                {
                    writer.WriteStartElement("cbc:AllowanceTotalAmount");
                    {
                        writer.WriteAttributeString("currencyID", LegalMonetaryTotal.AllowanceTotalAmount.CurrencyId);
                        writer.WriteValue(LegalMonetaryTotal.AllowanceTotalAmount.Value.ToString(Formatos.FormatoNumerico, Formato));
                    }
                    writer.WriteEndElement();
                }
                if (LegalMonetaryTotal.PrepaidAmount.Value > 0)
                {
                    writer.WriteStartElement("cbc:PrepaidAmount");
                    {
                        writer.WriteAttributeString("currencyID", LegalMonetaryTotal.PrepaidAmount.CurrencyId);
                        writer.WriteValue(LegalMonetaryTotal.PrepaidAmount.Value.ToString(Formatos.FormatoNumerico, Formato));
                    }
                    writer.WriteEndElement();
                }
                writer.WriteStartElement("cbc:PayableAmount");
                {
                    writer.WriteAttributeString("currencyID", LegalMonetaryTotal.PayableAmount.CurrencyId);
                    writer.WriteValue(LegalMonetaryTotal.PayableAmount.Value.ToString(Formatos.FormatoNumerico, Formato));
                }
                writer.WriteEndElement();

            }
            writer.WriteEndElement();
            #endregion

            #region InvoiceLines
            foreach (var invoiceLine in InvoiceLines)
            {
                writer.WriteStartElement("cac:InvoiceLine");

                writer.WriteElementString("cbc:ID", invoiceLine.Id.ToString());

                #region InvoicedQuantity
                writer.WriteStartElement("cbc:InvoicedQuantity");
                writer.WriteAttributeString("unitCode", invoiceLine.InvoicedQuantity.UnitCode);
                writer.WriteValue(invoiceLine.InvoicedQuantity.Value.ToString(Formatos.FormatoNumerico, Formato));
                writer.WriteEndElement();
                #endregion

                #region LineExtensionAmount
                writer.WriteStartElement("cbc:LineExtensionAmount");
                writer.WriteAttributeString("currencyID", invoiceLine.LineExtensionAmount.CurrencyId);
                writer.WriteValue(invoiceLine.LineExtensionAmount.Value.ToString(Formatos.FormatoNumerico, Formato));
                writer.WriteEndElement();
                #endregion

                #region PricingReference
                writer.WriteStartElement("cac:PricingReference");

                #region AlternativeConditionPrice
                foreach (var item in invoiceLine.PricingReference.AlternativeConditionPrices)
                {
                    writer.WriteStartElement("cac:AlternativeConditionPrice");

                    #region PriceAmount
                    writer.WriteStartElement("cbc:PriceAmount");
                    writer.WriteAttributeString("currencyID", item.PriceAmount.CurrencyId);
                    writer.WriteValue(item.PriceAmount.Value.ToString(Formatos.FormatoNumerico, Formato));
                    writer.WriteEndElement();
                    #endregion

                    writer.WriteElementString("cbc:PriceTypeCode", item.PriceTypeCode);

                    writer.WriteEndElement();
                }
                #endregion

                writer.WriteEndElement();
                #endregion

                #region AllowanceCharge
                if (invoiceLine.AllowanceCharge.Amount.Value > 0)
                {
                    writer.WriteStartElement("cac:AllowanceCharge");

                    writer.WriteElementString("cbc:ChargeIndicator", invoiceLine.AllowanceCharge.ChargeIndicator.ToString().ToLower());

                    #region Amount
                    writer.WriteStartElement("cbc:Amount");
                    writer.WriteAttributeString("currencyID", invoiceLine.AllowanceCharge.Amount.CurrencyId);
                    writer.WriteValue(invoiceLine.AllowanceCharge.Amount.Value.ToString(Formatos.FormatoNumerico, Formato));
                    writer.WriteEndElement();
                    #endregion

                    writer.WriteEndElement();
                }
                #endregion

                #region TaxTotal
                {
                    foreach (var taxTotal in invoiceLine.TaxTotals)
                    {
                        writer.WriteStartElement("cac:TaxTotal");

                        writer.WriteStartElement("cbc:TaxAmount");
                        writer.WriteAttributeString("currencyID", taxTotal.TaxAmount.CurrencyId);
                        writer.WriteString(taxTotal.TaxAmount.Value.ToString(Formatos.FormatoNumerico, Formato));
                        writer.WriteEndElement();

                        #region TaxSubtotal
                        writer.WriteStartElement("cac:TaxSubtotal");

                        #region TaxableAmount

                        if (!string.IsNullOrEmpty(taxTotal.TaxableAmount.CurrencyId))
                        {
                            writer.WriteStartElement("cbc:TaxableAmount");
                            writer.WriteAttributeString("currencyID", taxTotal.TaxableAmount.CurrencyId);
                            writer.WriteString(taxTotal.TaxableAmount.Value.ToString(Formatos.FormatoNumerico, Formato));
                            writer.WriteEndElement();
                        }

                        #endregion

                        writer.WriteStartElement("cbc:TaxAmount");
                        writer.WriteAttributeString("currencyID", taxTotal.TaxSubtotal.TaxAmount.CurrencyId);
                        writer.WriteString(taxTotal.TaxAmount.Value.ToString(Formatos.FormatoNumerico, Formato));
                        writer.WriteEndElement();
                        if (taxTotal.TaxSubtotal.Percent > 0)
                            writer.WriteElementString("cbc:Percent", taxTotal.TaxSubtotal.Percent.ToString(Formatos.FormatoNumerico, Formato));

                        #region TaxCategory
                        writer.WriteStartElement("cac:TaxCategory");
                        //writer.WriteElementString("cbc:ID", invoiceLine.TaxTotal.TaxSubtotal.TaxCategory.ID);
                        writer.WriteElementString("cbc:TaxExemptionReasonCode", taxTotal.TaxSubtotal.TaxCategory.TaxExemptionReasonCode);
                        if (!string.IsNullOrEmpty(taxTotal.TaxSubtotal.TaxCategory.TierRange))
                            writer.WriteElementString("cbc:TierRange", taxTotal.TaxSubtotal.TaxCategory.TierRange);

                        #region TaxScheme
                        {
                            writer.WriteStartElement("cac:TaxScheme");

                            writer.WriteElementString("cbc:ID", taxTotal.TaxSubtotal.TaxCategory.TaxScheme.Id);
                            writer.WriteElementString("cbc:Name", taxTotal.TaxSubtotal.TaxCategory.TaxScheme.Name);
                            writer.WriteElementString("cbc:TaxTypeCode", taxTotal.TaxSubtotal.TaxCategory.TaxScheme.TaxTypeCode);

                            writer.WriteEndElement();
                        }
                        #endregion

                        writer.WriteEndElement();
                        #endregion

                        writer.WriteEndElement();
                        #endregion

                        writer.WriteEndElement();
                    }
                }
                #endregion

                #region Item
                writer.WriteStartElement("cac:Item");

                #region Description
                writer.WriteElementString("cbc:Description", invoiceLine.Item.Description);
                //writer.WriteStartElement("cbc:Description");
                //writer.WriteCData(invoiceLine.Item.Description);
                //writer.WriteEndElement();
                #endregion

                #region SellersItemIdentification
                writer.WriteStartElement("cac:SellersItemIdentification");
                writer.WriteElementString("cbc:ID", invoiceLine.Item.SellersItemIdentification.Id);
                writer.WriteEndElement();
                #endregion


                writer.WriteEndElement();
                #endregion

                #region Price
                writer.WriteStartElement("cac:Price");

                writer.WriteStartElement("cbc:PriceAmount");
                writer.WriteAttributeString("currencyID", invoiceLine.Price.PriceAmount.CurrencyId);
                writer.WriteString(invoiceLine.Price.PriceAmount.Value.ToString(Formatos.FormatoNumerico, Formato));
                writer.WriteEndElement();

                writer.WriteEndElement();
                #endregion

                writer.WriteEndElement();
            }
            #endregion

        }
    }
}
