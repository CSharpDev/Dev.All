/*
 * Licensed to Jasig under one or more contributor license
 * agreements. See the NOTICE file distributed with this work
 * for additional information regarding copyright ownership.
 * Jasig licenses this file to you under the Apache License,
 * Version 2.0 (the "License"); you may not use this file
 * except in compliance with the License.  You may obtain a
 * copy of the License at the following location:
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */
//package org.jasig.cas.authentication.handler.support;

//import org.jasig.cas.authentication.handler.AuthenticationException;
//import org.jasig.cas.authentication.handler.NamedAuthenticationHandler;
//import org.jasig.cas.authentication.principal.Credentials;
//import org.slf4j.Logger;
//import org.slf4j.LoggerFactory;

//import javax.validation.constraints.NotNull;

/**
 * Abstract authentication handler that allows deployers to utilize the bundled
 * AuthenticationHandlers while providing a mechanism to perform tasks before
 * and after authentication.
 * 
 * @author Scott Battaglia
 * @version $Revision$ $Date$
 * @since 3.1
 */
public abstract class AbstractPreAndPostProcessingAuthenticationHandler
    : NamedAuthenticationHandler {
    
    /** Instance of logging for subclasses. */
    protected Logger log = LoggerFactory.getLogger(this.getClass());
    
    /** The name of the authentication handler. */
    @NotNull
    private string name = getClass().getName();

    /**
     * Method to execute before authentication occurs.
     * 
     * @param credentials the Credentials supplied
     * @return true if authentication should continue, false otherwise.
     */
    protected bool preAuthenticate( Credentials credentials) {
        return true;
    }

    /**
     * Method to execute after authentication occurs.
     * 
     * @param credentials the supplied credentials
     * @param authenticated the result of the authentication attempt.
     * @return true if the handler should return true, false otherwise.
     */
    protected bool postAuthenticate( Credentials credentials,
         bool authenticated) {
        return authenticated;
    }
    
    public  void setName( string name) {
        this.name = name;
    }
    
    public  string getName() {
        return this.name;
    }

    public  bool authenticate( Credentials credentials)
        throws AuthenticationException {

        if (!preAuthenticate(credentials)) {
            return false;
        }

         bool authenticated = doAuthentication(credentials);

        return postAuthenticate(credentials, authenticated);
    }

    protected abstract bool doAuthentication( Credentials credentials)
        throws AuthenticationException;
}