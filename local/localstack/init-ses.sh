#!/bin/bash

echo "⏳ Verificando e-mail SES no LocalStack..."
awslocal ses verify-email-identity --email-address teste@exemplo.com
echo "✅ E-mail verificado com sucesso!"
