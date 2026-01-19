/*
 * Copyright (c) 2026 Oliver Tattersfield
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

/**
 * AuthModels.cs - Data models and DTOs for authentication operations.
 * Contains request/response models for login, registration, and authentication-related data transfer.
 */

// Models/AuthDtos.cs
namespace ApiTemplateControllers.Models
{
    /// <summary>
    /// Represents a user login request containing email and password credentials.
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// Gets or sets the user's email address for authentication.
        /// </summary>
        public string Email { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the user's password for authentication.
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents a successful login response containing authentication token and user information.
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// Gets or sets the JWT authentication token.
        /// </summary>
        public string Token { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the authenticated user's email address.
        /// </summary>
        public string Email { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the authenticated user's unique identifier.
        /// </summary>
        public long UserId { get; set; }
        
        /// <summary>
        /// Gets or sets the token expiration date and time.
        /// </summary>
        public DateTime ExpiresAt { get; set; }
    }
}