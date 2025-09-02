import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, take, switchMap } from 'rxjs/operators';
import { ApiConfigService } from './api-config.service';

export interface Publisher {
  id: string;
  companyName: string;
  homePage?: string;
  description?: string;
}

@Injectable({
  providedIn: 'root'
})
export class PublisherService {
  constructor(
    private http: HttpClient,
    private apiConfig: ApiConfigService
  ) {}

  getAllPublishers(): Observable<Publisher[]> {
    return this.apiConfig.config$.pipe(
      filter(config => config !== null),
      take(1),
      switchMap(config => {
        const url = this.apiConfig.buildUrl(config!.publishersApiUrl);
        return this.http.get<Publisher[]>(url);
      })
    );
  }

  getPublisherByCompanyName(companyName: string): Observable<Publisher> {
    const config = this.apiConfig.getConfig();
    if (!config) {
      throw new Error('API configuration not loaded');
    }

    const url = this.apiConfig.buildUrl(config.publisherApiUrl, { companyname: companyName });
    return this.http.get<Publisher>(url);
  }

  getPublisherByGame(gameKey: string): Observable<Publisher> {
    const config = this.apiConfig.getConfig();
    if (!config) {
      throw new Error('API configuration not loaded');
    }

    const url = this.apiConfig.buildUrl(config.publisherByGameApiUrl, { key: gameKey });
    return this.http.get<Publisher>(url);
  }
}
