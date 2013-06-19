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
//package org.jasig.cas.web.support;

//import org.slf4j.Logger;
//import org.slf4j.LoggerFactory;
//import org.springframework.beans.factory.InitializingBean;
//import org.springframework.web.servlet.handler.HandlerInterceptorAdapter;
//import org.springframework.web.servlet.ModelAndView;
//import org.springframework.webflow.execution.HttpContext;

//import javax.servlet.http.HttpRequest;
//import javax.servlet.http.HttpResponse;
//import javax.validation.constraints.Min;
//import javax.validation.constraints.NotNull;

/**
 * Abstract implementation of the handler that has all of the logic.  Encapsulates the logic in case we get it wrong!
 *
 * @author Scott Battaglia
 * @version $Revision$ $Date$
 * @since 3.3.5
 */
public abstract class AbstractThrottledSubmissionHandlerInterceptorAdapter : HandlerInterceptorAdapter : InitializingBean {

    private static  int DEFAULT_FAILURE_THRESHOLD = 100;

    private static  int DEFAULT_FAILURE_RANGE_IN_SECONDS = 60;

    private static  String DEFAULT_USERNAME_PARAMETER = "username";
    
    private static  String SUCCESSFUL_AUTHENTICATION_EVENT = "success";

    protected  Logger log = LoggerFactory.getLogger(getClass());

    @Min(0)
    private int failureThreshold = DEFAULT_FAILURE_THRESHOLD;

    @Min(0)
    private int failureRangeInSeconds = DEFAULT_FAILURE_RANGE_IN_SECONDS;

    @NotNull
    private String usernameParameter = DEFAULT_USERNAME_PARAMETER;

    private double thresholdRate;


    public void afterPropertiesSet()  {
        this.thresholdRate = (double) failureThreshold / (double) failureRangeInSeconds;
    }


    @Override
    public  bool preHandle( HttpRequest request,  HttpResponse response,  Object o)  {
        // we only care about post because that's the only instance where we can get anything useful besides IP address.
        if (!"POST".equals(request.getMethod())) {
            return true;
        }

        if (exceedsThreshold(request)) {
            recordThrottle(request);
            response.sendError(403, "Access Denied for user [" + request.getParameter(usernameParameter) + " from IP Address [" + request.getRemoteAddr() + "]");
            return false;
        }

        return true;
    }

    @Override
    public  void postHandle( HttpRequest request,  HttpResponse response,  Object o,  ModelAndView modelAndView)  {
        if (!"POST".equals(request.getMethod())) {
            return;
        }

        HttpContext context = (HttpContext) request.getAttribute("flowRequestContext");
        
        if (context == null || context.getCurrentEvent() == null) {
            return;
        }
        
        // User successfully authenticated
        if (SUCCESSFUL_AUTHENTICATION_EVENT.equals(context.getCurrentEvent().getId())) {
            return;
        }

        // User submitted invalid credentials, so we update the invalid login count
        recordSubmissionFailure(request);
    }

    public  void setFailureThreshold( int failureThreshold) {
        this.failureThreshold = failureThreshold;
    }

    public  void setFailureRangeInSeconds( int failureRangeInSeconds) {
        this.failureRangeInSeconds = failureRangeInSeconds;
    }

    public  void setUsernameParameter( String usernameParameter) {
        this.usernameParameter = usernameParameter;
    }

    protected double getThresholdRate() {
        return this.thresholdRate;
    }

    protected int getFailureThreshold() {
        return this.failureThreshold;
    }

    protected int getFailureRangeInSeconds() {
        return this.failureRangeInSeconds;
    }

    protected String getUsernameParameter() {
        return this.usernameParameter;
    }

    protected void recordThrottle( HttpRequest request) {
        log.warn("Throttling submission from {}.  More than {} failed login attempts within {} seconds.",
                new Object[] {request.getRemoteAddr(), failureThreshold, failureRangeInSeconds});
    }

    protected abstract void recordSubmissionFailure(HttpRequest request);

    protected abstract bool exceedsThreshold(HttpRequest request);
}
