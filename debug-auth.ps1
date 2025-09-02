$baseUrl = "https://localhost:7281"

Write-Host "🔍 Detailed Authentication Debugging..." -ForegroundColor Yellow
Write-Host ""

# First, login to get token
Write-Host "1️⃣  Getting token..." -ForegroundColor Cyan
$loginRequest = @{
    model = @{
        login = "admin@gamestore.com"
        password = "Admin123!"
        internalAuth = $true
    }
} | ConvertTo-Json -Depth 3
 
try {
    $loginResponse = Invoke-RestMethod -Uri "$baseUrl/users/login" -Method POST -Body $loginRequest -ContentType "application/json" -SkipCertificateCheck
    
    if ($loginResponse.success) {
        Write-Host "✅ Login successful!" -ForegroundColor Green
        $token = $loginResponse.token
        Write-Host "   Token length: $($token.Length)" -ForegroundColor Gray
        Write-Host "   Token preview: $($token.Substring(0, [Math]::Min(50, $token.Length)))..." -ForegroundColor Gray
    } else {
        Write-Host "❌ Login failed" -ForegroundColor Red
        exit 1
    }
} catch {
    Write-Host "❌ Login request failed: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

Write-Host ""

# Create headers with explicit Bearer prefix
$authHeaderValue = "Bearer $token"
Write-Host "2️⃣  Authorization header being sent:" -ForegroundColor Cyan
Write-Host "   Full header: $($authHeaderValue.Substring(0, [Math]::Min(70, $authHeaderValue.Length)))..." -ForegroundColor Gray
Write-Host "   Starts with 'Bearer ': $($authHeaderValue.StartsWith('Bearer '))" -ForegroundColor Gray

$headers = @{
    "Authorization" = $authHeaderValue
}

Write-Host ""

# Test with explicit verbose output
Write-Host "3️⃣  Testing authenticated endpoint with detailed tracing..." -ForegroundColor Cyan

try {
    # Use Invoke-WebRequest for more detailed debugging
    $response = Invoke-WebRequest -Uri "$baseUrl/users/test-with-auth" -Method GET -Headers $headers -SkipCertificateCheck
    
    Write-Host "✅ Request successful!" -ForegroundColor Green
    Write-Host "   Status Code: $($response.StatusCode)" -ForegroundColor Gray
    
    $content = $response.Content | ConvertFrom-Json
    Write-Host "   User ID: $($content.userId)" -ForegroundColor Gray
    Write-Host "   Email: $($content.email)" -ForegroundColor Gray
    
} catch {
    Write-Host "❌ Request failed!" -ForegroundColor Red
    Write-Host "   Error: $($_.Exception.Message)" -ForegroundColor Red
    
    if ($_.Exception.Response) {
        Write-Host "   Status Code: $($_.Exception.Response.StatusCode)" -ForegroundColor Red
        Write-Host "   Status Description: $($_.Exception.Response.StatusDescription)" -ForegroundColor Red
        
        # Try to read response content
        try {
            $stream = $_.Exception.Response.GetResponseStream()
            $reader = New-Object System.IO.StreamReader($stream)
            $responseBody = $reader.ReadToEnd()
            if ($responseBody) {
                Write-Host "   Response Body: $responseBody" -ForegroundColor Red
            }
        } catch {
            Write-Host "   Could not read response body" -ForegroundColor Red
        }
    }
}

Write-Host ""

# Test token validation manually
Write-Host "4️⃣  Manual token validation..." -ForegroundColor Cyan
$validateRequest = $token | ConvertTo-Json

try {
    $validateResponse = Invoke-RestMethod -Uri "$baseUrl/users/debug/validate-token" -Method POST -Body $validateRequest -ContentType "application/json" -SkipCertificateCheck
    
    Write-Host "✅ Token validation response:" -ForegroundColor Green
    Write-Host "   Is Valid: $($validateResponse.isValid)" -ForegroundColor Gray
    Write-Host "   Is Expired: $($validateResponse.isExpired)" -ForegroundColor Gray
    Write-Host "   Current Time: $($validateResponse.currentTime)" -ForegroundColor Gray
    Write-Host "   Expiry Time: $($validateResponse.expiryTime)" -ForegroundColor Gray
    
    if ($validateResponse.decodedClaims) {
        Write-Host "   Claims count: $($validateResponse.decodedClaims.Length)" -ForegroundColor Gray
        foreach ($claim in $validateResponse.decodedClaims) {
            Write-Host "      $($claim.type): $($claim.value)" -ForegroundColor DarkGray
        }
    }
    
} catch {
    Write-Host "❌ Token validation failed: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""

# Test using curl to see raw HTTP request
Write-Host "5️⃣  Testing with curl for raw HTTP debugging..." -ForegroundColor Cyan
Write-Host "   Running: curl -v -H 'Authorization: Bearer [token]' https://localhost:7281/users/test-with-auth" -ForegroundColor Gray

$curlCommand = "curl -k -v -H `"Authorization: Bearer $token`" `"$baseUrl/users/test-with-auth`""
Write-Host "   Executing curl command..." -ForegroundColor Gray

try {
    $curlResult = Invoke-Expression $curlCommand 2>&1
    Write-Host "   Curl output:" -ForegroundColor Gray
    $curlResult | ForEach-Object { Write-Host "      $_" -ForegroundColor DarkGray }
} catch {
    Write-Host "   Curl failed: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""
Write-Host "🏁 Detailed debugging completed!" -ForegroundColor Yellow 