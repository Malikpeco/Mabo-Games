// === COMMANDS (WRITE) ===

import { PageResult } from "../../core/models/paging/page-result";

/**
 * Command for POST /register
 * Corresponds to: RegisterUserSecurityQuestionCommand.cs
 */
export interface RegisterUserSecurityQuestionCommand {
    securityQuestionId: number;
    securityQuestionAnswer: string;
}

/**
 * Command for DELETE /{id}
 * Corresponds to: RemoveUserSecurityQuestionCommand.cs
 */
export interface RemoveUserSecurityQuestionCommand {
    userSecurityQuestionId: number;
}


/**
 * Command for PUT /{id}
 * Corresponds to: UpdateUserSecurityQuestionCommand.cs
 */
export interface UpdateUserSecurityQuestionCommand {
    userSecurityQuestionId: number;
    newAnswer: string;
}



// === QUERIES (READ) ===



/**
 * Response for GET /{id}
 * Corresponds to: GetUserSecurityQuestionsByIdQuery.cs
 */

export interface GetUserSecurityQuestionsByIdQuery{
    securityQuestionId: number;
}

/**
 * Response for GET /{id}
 * Corresponds to: GetUserSecurityQuestionsByIdQuery.cs
 */

export interface GetUserSecurityQuestionsByIdQueryDto{
    securityQuestionId: number;
    securityQuestion: string;
}


/**
 * Response for GET /list
 * Corresponds to: GetUserSecurityQuestionsByListQueryDto.cs
 */

export interface GetUserSecurityQuestionsListQueryDto{
    securityQuestionId: number;
    securityQuestion: string;
}


/**
 * Response for GET /list-email
 * Corresponds to: GetUserSecurityQuestionsByListQueryDto.cs
 */

export interface GetUserSecurityQuestionsListByEmailQuery{
    userEmail: string;
}


/**
 * Response for GET /list-email
 * Corresponds to: GetUserSecurityQuestionsByListQueryDto.cs
 */

export interface GetUserSecurityQuestionsListByEmailQueryDto{
        id: number;
        question: string;
}




