// core/company/services/company-api.service.ts
import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import {
    CompanyDashboardResponse,
    CompanyResponse,
    CompleteBasicInformationRequest,
    CompleteBrandingRequest,
    CompleteContactInformationRequest,
    CompleteFiscalInformationRequest,
    InviteUserRequest,
    RegisterCompanyRequest,
    RegisterCompanyResponse,
    UpdateCompanyRequest,
} from '../models/company.models';

@Injectable({ providedIn: 'root' })
export class CompanyApiService {
    private readonly http = inject(HttpClient);

    // ASSUMPTION: environment.companyServiceUrl aponta para o CompanyService
    // (ex: 'https://localhost:5105'). Ajusta quando o Ocelot estiver ativo.
    private readonly baseUrl = `${environment.companyService}/api/companies`;

    register(request: RegisterCompanyRequest): Observable<RegisterCompanyResponse> {
        return this.http.post<RegisterCompanyResponse>(this.baseUrl, request);
    }

    getById(companyId: string): Observable<CompanyResponse> {
        return this.http.get<CompanyResponse>(`${this.baseUrl}/${companyId}`);
    }

    getAll(): Observable<CompanyResponse[]> {
        return this.http.get<CompanyResponse[]>(this.baseUrl);
    }

    getCurrent(): Observable<CompanyResponse> {
        return this.http.get<CompanyResponse>(`${this.baseUrl}/current`);
    }

    getDashboard(companyId: string): Observable<CompanyDashboardResponse> {
        return this.http.get<CompanyDashboardResponse>(`${this.baseUrl}/${companyId}/dashboard`);
    }

    update(request: UpdateCompanyRequest): Observable<void> {
        return this.http.put<void>(this.baseUrl, request);
    }

    inviteUser(companyId: string, request: InviteUserRequest): Observable<void> {
        return this.http.post<void>(`${this.baseUrl}/${companyId}/users/invite`, request);
    }

    // Wizard
    completeBasicInformation(companyId: string, request: CompleteBasicInformationRequest): Observable<void> {
        return this.http.put<void>(`${this.baseUrl}/${companyId}/wizard/basic-information`, request);
    }

    completeContactInformation(companyId: string, request: CompleteContactInformationRequest): Observable<void> {
        return this.http.put<void>(`${this.baseUrl}/${companyId}/wizard/contact-information`, request);
    }

    completeFiscalInformation(companyId: string, request: CompleteFiscalInformationRequest): Observable<void> {
        return this.http.put<void>(`${this.baseUrl}/${companyId}/wizard/fiscal-information`, request);
    }

    completeBranding(companyId: string, request: CompleteBrandingRequest): Observable<void> {
        return this.http.put<void>(`${this.baseUrl}/${companyId}/wizard/branding`, request);
    }
}