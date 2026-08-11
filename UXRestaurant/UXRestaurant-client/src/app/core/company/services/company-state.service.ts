// core/company/services/company-state.service.ts
import { Injectable, signal, computed } from '@angular/core';
import { CompanyResponse } from '../models/company.models';

@Injectable({ providedIn: 'root' })
export class CompanyStateService {
    private readonly _companies = signal<CompanyResponse[]>([]);
    private readonly _activeCompany = signal<CompanyResponse | null>(null);
    private readonly _loading = signal<boolean>(false);
    private readonly _error = signal<string | null>(null);

    readonly companies = this._companies.asReadonly();
    readonly activeCompany = this._activeCompany.asReadonly();
    readonly loading = this._loading.asReadonly();
    readonly error = this._error.asReadonly();

    readonly hasCompanies = computed(() => this._companies().length > 0);
    readonly hasActiveCompany = computed(() => this._activeCompany() !== null);

    setCompanies(companies: CompanyResponse[]): void {
        this._companies.set(companies);
    }

    setActiveCompany(company: CompanyResponse | null): void {
        this._activeCompany.set(company);
    }

    upsertCompany(company: CompanyResponse): void {
        const current = this._companies();
        const index = current.findIndex(c => c.companyId === company.companyId);

        if (index === -1) {
            this._companies.set([...current, company]);
        } else {
            const updated = [...current];
            updated[index] = company;
            this._companies.set(updated);
        }

        if (this._activeCompany()?.companyId === company.companyId) {
            this._activeCompany.set(company);
        }
    }

    setLoading(loading: boolean): void {
        this._loading.set(loading);
    }

    setError(error: string | null): void {
        this._error.set(error);
    }

    clear(): void {
        this._companies.set([]);
        this._activeCompany.set(null);
        this._error.set(null);
    }
}