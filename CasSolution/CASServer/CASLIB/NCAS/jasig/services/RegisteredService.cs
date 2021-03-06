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

using System;
using System.Collections.Generic;
using NCAS.jasig.authentication.principal;

/**
 * Interface for a service that can be registered by the Services Management
 * interface.
 * 
 * @author Scott Battaglia
 * @version $Revision$ $Date$
 * @since 3.1
 */
namespace NCAS.jasig.services
{
    public interface RegisteredService : ICloneable
    {
        /**
     * Is this application currently allowed to use CAS?
     * 
     * @return true if it can use CAS, false otherwise.
     */
        bool isEnabled();

        /**
     * Determines whether the service is allowed anonymous or privileged access
     * to user information. Anonymous access should not return any identifying
     * information such as user id.
     *
     * @return if we should use a pseudo random identifier instead of their real id
     */
        bool isAnonymousAccess();

        /**
     * Sets whether we should bother to read the attribute list or not.
     * 
     * @return true if we should read it, false otherwise.
     */
        bool isIgnoreAttributes();

        /**
     * Returns the list of allowed attributes.
     * 
     * @return the list of attributes
     */
        List<string> getAllowedAttributes();

        /**
     * Is this application allowed to take part in the proxying capabilities of
     * CAS?
     * 
     * @return true if it can, false otherwise.
     */
        bool isAllowedToProxy();

        /**
     * The unique identifier for this service.
     * 
     * @return the unique identifier for this service.
     */
        string getServiceId();

        /**
     * The numeric identifier for this service.
     * 
     * @return the numeric identifier for this service.
     */
        long getId();

        /**
     * Returns the name of the service.
     * 
     * @return the name of the service.
     */
        string getName();

        /**
     * Returns a short theme name. Services do not need to have unique theme
     * names.
     * 
     * @return the theme name associated with this service.
     */
        string getTheme();

        /**
     * Does this application participate in the SSO session?
     * 
     * @return true if it does, false otherwise.
     */
        bool isSsoEnabled();

        /**
     * Returns the description of the service.
     * 
     * @return the description of the service.
     */
        string getDescription();

        /**
     * Gets the relative evaluation order of this service when determining
     * matches.
     * @return Evaluation order relative to other registered services.
     * Services with lower values will be evaluated for a match before others.
     */
        int getEvaluationOrder();

        /**
     * Sets the relative evaluation order of this service when determining
     * matches.
     */
        void setEvaluationOrder(int evaluationOrder);

        /**
     * Get the name of the attribute this service prefers to consume as username.
     * 
     * @return Either of the following values:
     * <ul>
     *  <li><code>string</code> representing the name of the attribute to consume as username</li>
     *  <li><code>null</code> indicating the default username</li>
     * </ul>
     */
        string getUsernameAttribute();

        /**
     * Returns whether the service matches the registered service.
     * <p>Note, as of 3.1.2, matches are case insensitive.
     * 
     * @param service the service to match.
     * @return true if they match, false otherwise.
     */
        bool matches(Service service);

        //Object clone();
    }
}
